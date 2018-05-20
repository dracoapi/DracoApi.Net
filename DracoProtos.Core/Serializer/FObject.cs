namespace DracoProtos.Core.Serializer
{
    public interface FObject
	{
		void ReadExternal(FInputStream stream);

		void WriteExternal(FOutputStream stream);
	}
}
