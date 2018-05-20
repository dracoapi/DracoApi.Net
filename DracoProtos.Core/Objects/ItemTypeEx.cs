using DracoProtos.Core.Enums;

namespace DracoProtos.Core.Objects
{
    public static class ItemTypeEx
	{
		public static bool IsRune(this ItemType type)
		{
			switch (type)
			{
			case ItemType.RUNE_1:
			case ItemType.RUNE_2:
			case ItemType.RUNE_3:
			case ItemType.RUNE_4:
			case ItemType.RUNE_5:
			case ItemType.RUNE_6:
				return true;
			default:
				return false;
			}
		}

		public static bool IsHeal(this ItemType type)
		{
			switch (type)
			{
			case ItemType.POTION_HEAL_1:
			case ItemType.POTION_HEAL_2:
			case ItemType.POTION_HEAL_3:
			case ItemType.POTION_HEAL_4:
				return true;
			default:
				return false;
			}
		}

		public static bool IsFood(this ItemType type)
		{
			switch (type)
			{
			case ItemType.FOOD_CATCH_CHANCE:
			case ItemType.FOOD_MORE_DUST:
			case ItemType.FOOD_CALM_DOWN:
			case ItemType.FOOD_MORE_CANDIES:
				return true;
			default:
				return false;
			}
		}

		public static bool IsBall(this ItemType type)
		{
			switch (type)
			{
			case ItemType.MAGIC_BALL_SIMPLE:
			case ItemType.MAGIC_BALL_NORMAL:
			case ItemType.MAGIC_BALL_GOOD:
				return true;
			default:
				return false;
			}
		}

		public static bool IsResurrection(this ItemType type)
		{
			return type == ItemType.POTION_RESURRECTION_1 || type == ItemType.POTION_RESURRECTION_2;
		}

		public static bool IsIncubator(this ItemType type)
		{
			switch (type)
			{
			case ItemType.INCUBATOR_1:
			case ItemType.INCUBATOR_3:
			case ItemType.INCUBATOR_PERPETUAL:
				return true;
			default:
				return false;
			}
		}

		public static bool IsPotion(this ItemType type)
		{
			return type.IsHeal() || type.IsResurrection();
		}
	}
}
