using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FShopConfigBase : FObject
	{
        public Dictionary<ArtifactName, int> artifacts;
        public ProductLot bagUpgrade;
        public Dictionary<string, ProductLot> coins;
        public ProductLot creatureStorageUpgrade;
        public Dictionary<string, FItemCreatureGroup> creatures;
        public Dictionary<string, ExtraPack> extraPacks;
        public Dictionary<string, float> marketFees;
        public List<ProductGroup> products;
        public List<SaleSetConfig> saleSets;

        public void ReadExternal(FInputStream stream)
		{
            this.artifacts = stream.ReadStaticMap<ArtifactName, int>(true, true);
            this.bagUpgrade = (ProductLot)stream.ReadStaticObject(typeof(ProductLot));
            this.coins = stream.ReadStaticMap<string, ProductLot>(true, true);
            this.creatureStorageUpgrade = (ProductLot)stream.ReadStaticObject(typeof(ProductLot));
            this.creatures = stream.ReadStaticMap<string, FItemCreatureGroup>(true, true);
            this.extraPacks = stream.ReadStaticMap<string, ExtraPack>(true, true);
            this.marketFees = stream.ReadStaticMap<string, float>(true, true);
            this.products = stream.ReadStaticList<ProductGroup>(true);
            this.saleSets = stream.ReadStaticList<SaleSetConfig>(true);
        }

        public void WriteExternal(FOutputStream stream)
		{
            stream.WriteStaticMap(this.artifacts, true, true);
            stream.WriteStaticObject(this.bagUpgrade);
            stream.WriteStaticMap(this.coins, true, true);
            stream.WriteStaticObject(this.creatureStorageUpgrade);
            stream.WriteStaticMap(this.creatures, true, true);
            stream.WriteStaticMap(this.extraPacks, true, true);
            stream.WriteStaticMap(this.marketFees, true, true);
            stream.WriteDynamicCollection(this.products, true);
            stream.WriteDynamicCollection(this.saleSets, true);
        }
    }
}
