using DracoProtos.Core.Base;
using DracoProtos.Core.Enums;
using System.Collections.Generic;

namespace DracoProtos.Core.Objects
{
    public class FLoot : FLootBase
	{
		public FLoot()
		{
			this.lootList = new List<FBaseLootItem>();
		}

		public int GetExp()
		{
			int result = 0;
			this.lootList.ForEach(delegate(FBaseLootItem entry)
			{
				if (entry is FLootItemExp)
				{
					result += entry.qty;
				}
			});
			return result;
		}

		public int GetDust()
		{
			int result = 0;
			this.lootList.ForEach(delegate(FBaseLootItem entry)
			{
				if (entry is FLootItemDust)
				{
					result += entry.qty;
				}
			});
			return result;
		}

		public List<ArtifactName> GetArtifacts()
		{
			List<ArtifactName> result = new List<ArtifactName>();
			this.lootList.ForEach(delegate(FBaseLootItem entry)
			{
				if (entry is FLootItemArtifact)
				{
					result.Add(((FLootItemArtifact)entry).artifact);
				}
			});
			return result;
		}

		public List<RecipeType> GetRecipes()
		{
			List<RecipeType> result = new List<RecipeType>();
			this.lootList.ForEach(delegate(FBaseLootItem entry)
			{
				if (entry is FLootItemRecipe)
				{
					result.Add(((FLootItemRecipe)entry).recipe);
				}
			});
			return result;
		}

		public Dictionary<ItemType, int> GetItems()
		{
			Dictionary<ItemType, int> result = new Dictionary<ItemType, int>();
			this.lootList.ForEach(delegate(FBaseLootItem entry)
			{
				if (entry is FLootItemItem)
				{
					FLootItemItem flootItemItem = (FLootItemItem)entry;
					if (result.ContainsKey(flootItemItem.item))
					{
						result[flootItemItem.item] = result[flootItemItem.item] + flootItemItem.qty;
					}
					else
					{
						result[flootItemItem.item] = flootItemItem.qty;
					}
				}
			});
			return result;
		}

		public Dictionary<InstantUseItem, int> GetInstantUseItems()
		{
			Dictionary<InstantUseItem, int> result = new Dictionary<InstantUseItem, int>();
			this.lootList.ForEach(delegate(FBaseLootItem entry)
			{
				if (entry is FLootItemInstantUseItem)
				{
					FLootItemInstantUseItem flootItemInstantUseItem = (FLootItemInstantUseItem)entry;
					if (result.ContainsKey(flootItemInstantUseItem.item))
					{
						result[flootItemInstantUseItem.item] = result[flootItemInstantUseItem.item] + flootItemInstantUseItem.qty;
					}
					else
					{
						result[flootItemInstantUseItem.item] = flootItemInstantUseItem.qty;
					}
				}
			});
			return result;
		}

		public int GetRunesCount()
		{
			int count = 0;
			this.lootList.ForEach(delegate(FBaseLootItem entry)
			{
				if (entry is FLootItemItem)
				{
					FLootItemItem flootItemItem = (FLootItemItem)entry;
					if (flootItemItem.item.IsRune())
					{
						count += flootItemItem.qty;
					}
				}
			});
			return count;
		}

		public void AddLoot(FLoot loot)
		{
			foreach (FBaseLootItem item in loot.lootList)
			{
				this.lootList.Add(item);
			}
		}

		public FLoot getGroupedLoot()
		{
			FLoot floot = new FLoot();
			Dictionary<string, FBaseLootItem> dictionary = new Dictionary<string, FBaseLootItem>();
			foreach (FBaseLootItem fbaseLootItem in this.lootList)
			{
				if (dictionary.ContainsKey(fbaseLootItem.GetLootGroup()))
				{
					FBaseLootItem fbaseLootItem2;
					dictionary.TryGetValue(fbaseLootItem.GetLootGroup(), out fbaseLootItem2);
					fbaseLootItem2.qty += fbaseLootItem.qty;
				}
				else
				{
					dictionary.Add(fbaseLootItem.GetLootGroup(), fbaseLootItem);
					floot.lootList.Add(fbaseLootItem);
				}
			}
			return floot;
		}

		public override void Handle()
		{
		}
	}
}
