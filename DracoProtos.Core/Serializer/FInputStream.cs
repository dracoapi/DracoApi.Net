using DracoProtos.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace DracoProtos.Core.Serializer
{
	public class FInputStream : IDisposable
	{
		public FInputStream(SerializerContext context, BinaryReader dataStream)
		{
			this.context = context;
			this.dataStream = dataStream;
		}

		public bool ReadBoolean()
		{
			return this.dataStream.ReadBoolean();
		}

		public sbyte ReadSByte()
		{
			return this.dataStream.ReadSByte();
		}

		public byte ReadByte()
		{
			return this.dataStream.ReadByte();
		}

		public short ReadShort()
		{
			return this.dataStream.ReadInt16();
		}

		public int ReadInt32()
		{
			return this.dataStream.ReadInt32();
		}

		public uint ReadUInt32()
		{
			return this.dataStream.ReadUInt32();
		}

		public long ReadInt64()
		{
			return this.dataStream.ReadInt64();
		}

		public float ReadFloat()
		{
			return this.dataStream.ReadSingle();
		}

		public double ReadDouble()
		{
			return this.dataStream.ReadDouble();
		}

		public string ReadUtfString()
		{
			int num = (int)this.dataStream.ReadByte();
			int num2 = (int)this.dataStream.ReadByte();
			int num3 = (num << 8) + (num2 << 0);
			if (this.utfByteBuff == null || this.utfByteBuff.Length < num3)
			{
				this.utfByteBuff = new byte[num3 * 2];
				this.utfCharBuff = new char[num3 * 2];
			}
			byte[] array = this.utfByteBuff;
			char[] array2 = this.utfCharBuff;
			int i = 0;
			int length = 0;
			FInputStream.ReadFully(this.dataStream, array, 0, num3);
			while (i < num3)
			{
				int num4 = (int)(array[i] & byte.MaxValue);
				if (num4 > 127)
				{
					break;
				}
				i++;
				array2[length++] = (char)num4;
			}
			while (i < num3)
			{
				int num4 = (int)(array[i] & byte.MaxValue);
				switch (num4 >> 4)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					i++;
					array2[length++] = (char)num4;
					continue;
				case 12:
				case 13:
				{
					i += 2;
					if (i > num3)
					{
						throw new Exception("malformed input: partial character at end");
					}
					int num5 = (int)array[i - 1];
					if ((num5 & 192) != 128)
					{
						throw new Exception("malformed input around byte " + i);
					}
					array2[length++] = (char)((num4 & 31) << 6 | (num5 & 63));
					continue;
				}
				case 14:
				{
					i += 3;
					if (i > num3)
					{
						throw new Exception("malformed input: partial character at end");
					}
					int num5 = (int)array[i - 2];
					int num6 = (int)array[i - 1];
					if ((num5 & 192) != 128 || (num6 & 192) != 128)
					{
						throw new Exception("malformed input around byte " + (i - 1));
					}
					array2[length++] = (char)((num4 & 15) << 12 | (num5 & 63) << 6 | (num6 & 63) << 0);
					continue;
				}
				}
				throw new Exception("malformed input around byte " + i);
			}
			return new string(array2, 0, length);
		}

		public Vector2 ReadVector2()
		{
			return new Vector2(this.ReadFloat(), this.ReadFloat());
		}

		public Enum ReadEnum(Type clazz)
		{
			int num = (int)this.ReadByte();
			Array values = Enum.GetValues(clazz);
			if (num < 0 || num >= values.Length)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Bad ordinal for enum ",
					clazz,
					" ordinal: ",
					num
				}));
			}
			return (Enum)values.GetValue(num);
		}

		public T[] ReadDynamicArray<T>(bool staticComponent)
		{
			sbyte b = this.ReadSByte();
			if ((int)b == 0)
			{
				return null;
			}
			return (T[])this.ReadDynamicArray(b, staticComponent);
		}

		private object ReadDynamicArray(sbyte id, bool staticComponent)
		{
			if ((int)id == 2)
			{
				sbyte classId = this.ReadSByte();
				Type type = this.context.FindClassById(classId);
				return this.ReadStaticArray(type, staticComponent);
			}
			if ((int)id == 3)
			{
				sbyte id2 = this.ReadSByte();
				Type arrayPrimitiveType = SerializerContext.GetArrayPrimitiveType(id2);
				return this.ReadStaticArray(arrayPrimitiveType, true);
			}
			throw new Exception("Object id is not array: " + id);
		}

		public T[] ReadStaticArray<T>(bool staticComponent)
		{
			int num = this.ReadLength();
			T[] array = new T[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = this.ReadObject<T>(staticComponent);
			}
			return array;
		}

		public object ReadStaticArray(Type type, bool staticComponent)
		{
			int num = this.ReadLength();
			Array array = Array.CreateInstance(type, num);
			for (int i = 0; i < num; i++)
			{
				array.SetValue(this.ReadObject(type, staticComponent), i);
			}
			return array;
		}

		private int ReadLength()
		{
			int num = (int)this.ReadShort();
			if ((num & 32768) != 0)
			{
				num &= -32769;
				int num2 = (int)this.ReadShort();
				return num << 16 | (num2 & 65535);
			}
			return num;
		}

		public Dictionary<K, V> ReadDynamicMap<K, V>(bool staticKey, bool staticValue)
		{
			sbyte b = this.ReadSByte();
			if ((int)b == 0)
			{
				return null;
			}
			return this.ReadStaticMap<K, V>(staticKey, staticValue);
		}

		public Dictionary<K, V> ReadStaticMap<K, V>(bool staticKey, bool staticValue)
		{
			int num = this.ReadLength();
			Dictionary<K, V> dictionary = new Dictionary<K, V>(num);
			for (int i = 0; i < num; i++)
			{
				K key = this.ReadObject<K>(staticKey);
				V value = this.ReadObject<V>(staticValue);
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		public SortedDictionary<K, V> ReadDynamicSortedMap<K, V>(bool staticKey, bool staticValue)
		{
			sbyte b = this.ReadSByte();
			if ((int)b == 0)
			{
				return null;
			}
			return this.ReadStaticSortedMap<K, V>(staticKey, staticValue);
		}

		public SortedDictionary<K, V> ReadStaticSortedMap<K, V>(bool staticKey, bool staticValue)
		{
			int num = this.ReadLength();
			SortedDictionary<K, V> sortedDictionary = new SortedDictionary<K, V>();
			for (int i = 0; i < num; i++)
			{
				K key = this.ReadObject<K>(staticKey);
				V value = this.ReadObject<V>(staticValue);
				sortedDictionary.Add(key, value);
			}
			return sortedDictionary;
		}

		public List<T> ReadDynamicList<T>(bool staticComponent)
		{
			sbyte b = this.ReadSByte();
			if ((int)b == 0)
			{
				return null;
			}
			return this.ReadStaticList<T>(staticComponent);
		}

		public List<T> ReadStaticList<T>(bool staticComponent)
		{
			int num = this.ReadLength();
			List<T> list = new List<T>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(this.ReadObject<T>(staticComponent));
			}
			return list;
		}

		public HashSet<T> ReadDynamicHashSet<T>(bool staticComponent)
		{
			sbyte b = this.ReadSByte();
			if ((int)b == 0)
			{
				return null;
			}
			return this.ReadStaticHashSet<T>(staticComponent);
		}

		public HashSet<T> ReadStaticHashSet<T>(bool staticComponent)
		{
			int num = this.ReadLength();
			HashSet<T> hashSet = new HashSet<T>();
			for (int i = 0; i < num; i++)
			{
				hashSet.Add(this.ReadObject<T>(staticComponent));
			}
			return hashSet;
		}

		private T ReadObject<T>(bool staticComponent)
		{
			return (T)((object)this.ReadObject(typeof(T), staticComponent));
		}

		private object ReadObject(Type type, bool staticComponent)
		{
			if (staticComponent)
			{
				return this.ReadStaticObject(type);
			}
			return this.ReadDynamicObject();
		}

		public object ReadDynamicObject()
		{
			sbyte b = this.ReadSByte();
			if ((int)b == 0)
			{
				return null;
			}
			if ((int)b == 2)
			{
				return this.ReadDynamicArray(b, false);
			}
			if ((int)b == 3)
			{
				return this.ReadDynamicArray(b, false);
			}
			Type clazz = this.context.FindClassById(b);
			return this.ReadStaticObject(clazz);
		}

		public object ReadStaticObject(Type clazz)
		{
			clazz = SerializerContext.ResolveClass(clazz);
			if (clazz == typeof(bool))
			{
				return this.ReadBoolean();
			}
			if (clazz == typeof(sbyte))
			{
				return this.ReadSByte();
			}
			if (clazz == typeof(short))
			{
				return this.ReadShort();
			}
			if (clazz == typeof(int))
			{
				return this.ReadInt32();
			}
			if (clazz == typeof(long))
			{
				return this.ReadInt64();
			}
			if (clazz == typeof(float))
			{
				return this.ReadFloat();
			}
			if (clazz == typeof(double))
			{
				return this.ReadDouble();
			}
			if (clazz == typeof(string))
			{
				return this.ReadUtfString();
			}
			if (clazz == typeof(List<>))
			{
				return this.ReadStaticList<object>(false);
			}
			if (clazz == typeof(HashSet<>))
			{
				return this.ReadStaticHashSet<object>(false);
			}
			if (clazz == typeof(SortedDictionary<, >))
			{
				return this.ReadStaticSortedMap<object, object>(false, false);
			}
			if (clazz == typeof(Dictionary<, >))
			{
				return this.ReadStaticMap<object, object>(false, false);
			}
			if (clazz.IsEnum)
			{
				return this.ReadEnum(clazz);
			}
			if (clazz == typeof(Vector2))
			{
				return this.ReadVector2();
			}
			if (clazz.IsArray)
			{
				Type elementType = clazz.GetElementType();
				return this.ReadStaticArray(elementType, elementType.IsPrimitive);
			}
            
			FObject fobject = Activator.CreateInstance(clazz) as FObject;
			if (fobject == null)
			{
				throw new Exception("Can't instantiate FObject of class: " + clazz);
			}
			object result;
			try
			{
				fobject.ReadExternal(this);
				result = fobject;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void Dispose()
		{
			this.dataStream.Close();
		}

		private static void ReadFully(BinaryReader reader, byte[] dest, int offset, int len)
		{
			int i = offset;
			int num = offset + len;
			while (i < num)
			{
				dest[i] = reader.ReadByte();
				i++;
			}
		}

		private readonly SerializerContext context;

		private readonly BinaryReader dataStream;

		private byte[] utfByteBuff;

		private char[] utfCharBuff;
	}
}
