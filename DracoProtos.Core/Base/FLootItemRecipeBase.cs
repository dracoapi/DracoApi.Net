using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootItemRecipeBase : FBaseLootItem
	{
        //public int qty;
        public int level;
        public RecipeType recipe;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            //this.qty = stream.ReadInt32();
            this.level = stream.ReadInt32();
			this.recipe = (RecipeType)stream.ReadEnum(typeof(RecipeType));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
            //.WriteInt32(this.qty);
            stream.WriteInt32(this.level);
			stream.WriteEnum(this.recipe);
		}
	}
}
