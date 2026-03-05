using System.Collections.Generic;
using System.Linq;

namespace PlayerSystemData
{
    public class BackpackSystem : BaseDataSystem
    {
        #region
        public static BackpackSystem Singleton
        {
            get{
                if (null == s_singleton) s_singleton = new BackpackSystem();
                return s_singleton;
            }
        }
        static BackpackSystem s_singleton;
        #endregion

        BackpackData backpackData = null;
        public override string SystemName { get { return "BackpackSystem"; } }
        public BackpackSystem(){
            backpackData = new BackpackData();
            systemData = backpackData;
        }

        public int GetItemNum(int itemId)
        {
            var item = GetItem(itemId);
            if(item == null)
                return 0;

            return item.itemCount;
        }

        //获取道具
        public ItemInfo GetItem(int itemId) {
            ItemInfo info = null;
            backpackData.items.TryGetValue(itemId, out info);

            return info;
        }

        //获取背包所有物品
        public List<ItemInfo> GetItemList()
        {
            return backpackData.items.Values.ToList();
        }

        public bool TryChangeItemNum(int itemId, int v)
        {
            ItemInfo item = GetItem(itemId);
            if (item == null)
            {
                item = new ItemInfo();
                item.itemXmlId = itemId;
            }

            var tarNum = item.itemCount + v;
            if(tarNum < 0)
                return false;

            item.itemCount = tarNum;
            backpackData.items[itemId] = item;

            if (item.itemCount == 0)
                backpackData.items.Remove(itemId);

            SystemMgr.SetDirt();
            EventUtils.OnBackPackChanged(itemId);
            return true;
        }
    }
}
