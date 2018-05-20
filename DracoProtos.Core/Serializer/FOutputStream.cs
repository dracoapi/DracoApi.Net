using DracoProtos.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DracoProtos.Core.Serializer
{
	public class FOutputStream : IDisposable
	{
		public FOutputStream(SerializerContext context, BinaryWriter dataStream)
		{
			this.context = context;
			this.dataStream = dataStream;
		}

		public void WriteBoolean(bool b)
		{
			this.dataStream.Write(b);
		}

		public void WriteSByte(sbyte b)
		{
			this.dataStream.Write(b);
		}

		public void WriteShort(short v)
		{
			this.dataStream.Write(v);
		}

		public void WriteInt32(int i)
		{
			this.dataStream.Write(i);
		}

		public void WriteInt64(long l)
		{
			this.dataStream.Write(l);
		}

		public void WriteFloat(float f)
		{
			this.dataStream.Write(f);
		}

		public void WriteDouble(double d)
		{
			this.dataStream.Write(d);
		}

		public void WriteUtfString(string str)
		{
			int length = str.Length;
			uint num = 0u;
			int num2 = 0;
			for (int i = 0; i < length; i++)
			{
				int num3 = (int)str[i];
				if (num3 >= 1 && num3 <= 127)
				{
					num += 1u;
				}
				else if (num3 > 2047)
				{
					num += 3u;
				}
				else
				{
					num += 2u;
				}
			}
			if (num > 65535u)
			{
				throw new Exception("encoded string too long: " + num + " bytes");
			}
			if (this.utfBuff == null || (long)this.utfBuff.Length < (long)((ulong)(num + 2u)))
			{
				this.utfBuff = new byte[num * 2u + 2u];
			}
			byte[] array = this.utfBuff;
			array[num2++] = (byte)(num >> 8 & 255u);
			array[num2++] = (byte)(num >> 0 & 255u);
			int j;
			for (j = 0; j < length; j++)
			{
				int num3 = (int)str[j];
				if (num3 < 1 || num3 > 127)
				{
					break;
				}
				array[num2++] = (byte)num3;
			}
			while (j < length)
			{
				int num3 = (int)str[j];
				if (num3 >= 1 && num3 <= 127)
				{
					array[num2++] = (byte)num3;
				}
				else if (num3 > 2047)
				{
					array[num2++] = (byte)(224 | (num3 >> 12 & 15));
					array[num2++] = (byte)(128 | (num3 >> 6 & 63));
					array[num2++] = (byte)(128 | (num3 >> 0 & 63));
				}
				else
				{
					array[num2++] = (byte)(192 | (num3 >> 6 & 31));
					array[num2++] = (byte)(128 | (num3 >> 0 & 63));
				}
				j++;
			}
			this.dataStream.Write(array, 0, (int)(num + 2u));
		}

		public void WriteVector2(Vector2 v)
		{
			this.dataStream.Write(v.x);
			this.dataStream.Write(v.y);
		}

		public void WriteEnum(object value)
		{
			int num = (int)value;
			this.WriteSByte((sbyte)num);
		}

		public void WriteDynamicMap(IDictionary map, bool staticKey, bool staticValue)
		{
			if (map == null)
			{
				this.WriteSByte(0);
			}
			else
			{
				sbyte b = this.context.FindIdByClass(SerializerContext.ResolveClass(map.GetType()));
				this.WriteSByte(b);
				this.WriteStaticMap(map, staticKey, staticValue);
			}
		}

		public void WriteStaticMap(IDictionary map, bool staticKey, bool staticValue)
		{
			this.WriteLength(map.Count);
			IDictionaryEnumerator enumerator = map.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					this.WriteObject(dictionaryEntry.Key, staticKey);
					this.WriteObject(dictionaryEntry.Value, staticValue);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public void WriteDynamicEnumerable(IEnumerable enumerable, bool staticComponent)
		{
			if (enumerable == null)
			{
				this.WriteSByte(0);
			}
			else
			{
				sbyte b = this.context.FindIdByClass(SerializerContext.ResolveClass(enumerable.GetType()));
				this.WriteSByte(b);
				this.WriteStaticEnumerable(enumerable, staticComponent);
			}
		}

		public void WriteStaticEnumerable(IEnumerable enumerable, bool staticComponent)
		{
			int length = enumerable.Cast<object>().Count<object>();
			this.WriteLength(length);
			IEnumerator enumerator = enumerable.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object o = enumerator.Current;
					this.WriteObject(o, staticComponent);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public void WriteDynamicCollection(ICollection collection, bool staticComponent)
		{
			if (collection == null)
			{
				this.WriteSByte(0);
			}
			else
			{
				sbyte b = this.context.FindIdByClass(SerializerContext.ResolveClass(collection.GetType()));
				this.WriteSByte(b);
				this.WriteStaticCollection(collection, staticComponent);
			}
		}

		public void WriteStaticCollection(ICollection collection, bool staticComponent)
		{
			this.WriteLength(collection.Count);
			IEnumerator enumerator = collection.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object o = enumerator.Current;
					this.WriteObject(o, staticComponent);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		private void WriteLength(int length)
		{
			if (length <= 32767)
			{
				this.WriteShort((short)length);
			}
			else
			{
				this.WriteShort((short)(((long)length & -65536) >> 16 | 32768L));
				this.WriteShort((short)(length & 65535));
			}
		}

		private void WriteObject(object o, bool staticComponent)
		{
			if (staticComponent)
			{
				this.WriteStaticObject(o);
			}
			else
			{
				this.WriteDynamicObject(o);
			}
		}

		public void WriteDynamicObject(object o)
		{
			if (o == null)
			{
				this.WriteSByte(0);
			}
			else if (o.GetType().IsArray)
			{
				sbyte arrayPrimitiveTypeId = SerializerContext.GetArrayPrimitiveTypeId(o);
				if ((int)arrayPrimitiveTypeId >= 0)
				{
					this.WriteSByte(3);
					this.WriteSByte(arrayPrimitiveTypeId);
					this.WriteStaticCollection((ICollection)o, true);
				}
				else
				{
					this.WriteSByte(2);
					sbyte b = this.context.FindIdByClass(o.GetType().GetElementType());
					this.WriteSByte(b);
					this.WriteStaticCollection((ICollection)o, false);
				}
			}
			else
			{
				sbyte b2 = this.context.FindIdByClass(SerializerContext.ResolveClass(o.GetType()));
				this.WriteSByte(b2);
				this.WriteStaticObject(o);
			}
		}

		public void WriteStaticObject(object o)
		{
			if (o == null)
			{
				throw new ArgumentException("Static object can't be null");
			}
			Type type = SerializerContext.ResolveClass(o.GetType());
			if (type == typeof(bool))
			{
				this.WriteBoolean((bool)o);
			}
			else if (type == typeof(sbyte))
			{
				this.WriteSByte((sbyte)o);
			}
			else if (type == typeof(short))
			{
				this.WriteShort((short)o);
			}
			else if (type == typeof(int))
			{
				this.WriteInt32((int)o);
			}
			else if (type == typeof(long))
			{
				this.WriteInt64((long)o);
			}
			else if (type == typeof(float))
			{
				this.WriteFloat((float)o);
			}
			else if (type == typeof(double))
			{
				this.WriteDouble((double)o);
			}
			else if (type == typeof(string))
			{
				this.WriteUtfString((string)o);
			}
			else if (type == typeof(List<>))
			{
				this.WriteStaticCollection((ICollection)o, false);
			}
			else if (type == typeof(HashSet<>))
			{
				this.WriteStaticEnumerable((IEnumerable)o, false);
			}
			else if (type == typeof(Dictionary<, >) || type == typeof(SortedDictionary<, >))
			{
				this.WriteStaticMap((IDictionary)o, false, false);
			}
			else if (type == typeof(Vector2))
			{
				this.WriteVector2((Vector2)o);
			}
			else if (type.IsArray)
			{
				this.WriteStaticEnumerable((IEnumerable)o, type.GetElementType().IsPrimitive);
			}
			else if (type.IsEnum)
			{
				this.WriteEnum(o);
			}
			else
			{
				if (!(o is FObject))
				{
					throw new Exception("can't write unknown object: " + type);
				}
				(o as FObject).WriteExternal(this);
			}
		}

		public void Dispose()
		{
			this.dataStream.Close();
		}

		private readonly SerializerContext context;

		private readonly BinaryWriter dataStream;

		private byte[] utfBuff;
	}
}
