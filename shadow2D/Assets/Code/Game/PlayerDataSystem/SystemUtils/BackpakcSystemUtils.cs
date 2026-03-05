using Game;
using System.Collections.Generic;

namespace PlayerSystemData
{
	static class BackpakcSystemUtils
    {

        public static bool TryChangeItemNum(int itemId, int num)
        {
            BackpackSystem system = SystemUtils.GetBackpackSystem();
            if (system == null)
                return false;

            return system.TryChangeItemNum(itemId, num);
        }

        static public ItemInfo GetItemInfoByItemID(int itemID)
        {
            BackpackSystem system = SystemUtils.GetBackpackSystem();
            if (system == null)
                return null;

            ItemInfo info = system.GetItem(itemID);
            return info;
        }

        static public List<ItemInfo> GetItemList() {
            BackpackSystem system = SystemUtils.GetBackpackSystem();
            if (system == null)
                return new List<ItemInfo>();
            return system.GetItemList();
        }
	}
}
