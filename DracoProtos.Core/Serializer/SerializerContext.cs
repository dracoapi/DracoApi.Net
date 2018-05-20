using DracoProtos.Core.Classes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DracoProtos.Core.Serializer
{
    public class SerializerContext
	{
		public SerializerContext(string name, Dictionary<Type, sbyte> map, uint protocolVersion)
		{
			this.name = name;
			this.protocolVersion = protocolVersion;
			foreach (KeyValuePair<Type, sbyte> keyValuePair in map)
			{
				this.id2ClassIndex.Add(keyValuePair.Value, keyValuePair.Key);
				this.class2IdIndex.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public Type FindClassById(sbyte classId)
		{
			Type result;
			if (this.id2ClassIndex.TryGetValue(classId, out result))
			{
				return result;
			}
			throw new ArgumentException("unknown classId: " + classId);
		}

		public sbyte FindIdByClass(Type clazz)
		{
			sbyte result;
			if (this.class2IdIndex.TryGetValue(clazz, out result))
			{
				return result;
			}
			throw new ArgumentException("unknown class: " + clazz);
		}

        public T Deserialize<T>(byte[] data)
        {
            FInputStream finputStream = new FInputStream(this, new BigEndianBinaryReader(new MemoryStream(data)));
            object obj = finputStream.ReadDynamicObject();

            return (T)obj;
        }

		public object Deserialize(byte[] data)
		{
            return Deserialize<object>(data);
		}

		public byte[] Serialize(object data)
		{
			MemoryStream memoryStream = new MemoryStream();
			new FOutputStream(this, new BigEndianBinaryWriter(memoryStream)).WriteDynamicObject(data);
			return memoryStream.ToArray();
		}

		public static Type ResolveClass(Type clazz)
		{
			if (clazz.IsGenericType && (clazz.GetGenericTypeDefinition() == typeof(List<>) || clazz.GetGenericTypeDefinition() == typeof(HashSet<>) || clazz.GetGenericTypeDefinition() == typeof(Dictionary<, >)))
			{
				return clazz.GetGenericTypeDefinition();
			}
			return clazz;
		}

		public static Type GetArrayPrimitiveType(sbyte id)
		{
			switch (id)
			{
			case 1:
				return typeof(byte);
			case 2:
				return typeof(short);
			case 3:
				return typeof(int);
			case 4:
				return typeof(long);
			case 5:
				return typeof(float);
			case 6:
				return typeof(double);
			case 7:
				return typeof(bool);
			default:
				throw new Exception("Wrong id: " + id);
			}
		}

		public static sbyte GetArrayPrimitiveTypeId(object array)
		{
			if (array is object[])
			{
				return -1;
			}
			if (array is bool[])
			{
				return 7;
			}
			if (array is byte[])
			{
				return 1;
			}
			if (array is short[])
			{
				return 2;
			}
			if (array is int[])
			{
				return 3;
			}
			if (array is long[])
			{
				return 4;
			}
			if (array is float[])
			{
				return 5;
			}
			if (array is double[])
			{
				return 6;
			}
			throw new Exception("Wrong type: " + array.GetType());
		}

		public static readonly SerializerContext Context = new SerializerContext("portal", FGameObjects.CLASSES, 839333433u);

		public const sbyte NULL = 0;

		public const sbyte REF = 1;

		public const sbyte ARRAY_OBJECT = 2;

		public const sbyte ARRAY_PRIMITIVE = 3;

		public const int BIG_LENGTH_BIT = 32768;

		public const sbyte ARRAY_PRIMITIVE_BYTE = 1;

		public const sbyte ARRAY_PRIMITIVE_SHORT = 2;

		public const sbyte ARRAY_PRIMITIVE_INT = 3;

		public const sbyte ARRAY_PRIMITIVE_LONG = 4;

		public const sbyte ARRAY_PRIMITIVE_FLOAT = 5;

		public const sbyte ARRAY_PRIMITIVE_DOUBLE = 6;

		public const sbyte ARRAY_PRIMITIVE_BOOLEAN = 7;

		public const sbyte ARRAY_PRIMITIVE_CHAT = 8;

		public readonly string name;

		public readonly uint protocolVersion;

		private readonly Dictionary<sbyte, Type> id2ClassIndex = new Dictionary<sbyte, Type>();

		private readonly Dictionary<Type, sbyte> class2IdIndex = new Dictionary<Type, sbyte>();
	}
}
