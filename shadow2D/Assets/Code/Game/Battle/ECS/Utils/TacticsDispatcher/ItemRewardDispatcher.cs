using Table;
using System.Collections.Generic;

namespace ECS
{
    public delegate void OnGiveItemFunction(Entity player,ItemBattleCfg  itemCfg, int count);

    public class ItemRewardDispatcher
    {
        private static Dictionary<int, OnGiveItemFunction> g_dictOnGiveItemFunctions = new Dictionary<int, OnGiveItemFunction>();

        public static void RegisterAllItemRewardFunction() {
            RegisterItemRewardFunction(ItemType.Exp, ItemRewardUitl.OnAddExp);
            RegisterItemRewardFunction(ItemType.Gold, ItemRewardUitl.OnAddGold);
            RegisterItemRewardFunction(ItemType.Skill, ItemRewardUitl.OnAddSkill);
            RegisterItemRewardFunction(ItemType.Status, ItemRewardUitl.OnAddStatus);
            RegisterItemRewardFunction(ItemType.UseSkill, ItemRewardUitl.OnUseSkill);
        }

        public static void ClearTactics()
        {
            g_dictOnGiveItemFunctions.Clear();
        }

        public static void RegisterItemRewardFunction(int itemType, OnGiveItemFunction moveFun)
        {
            if (g_dictOnGiveItemFunctions.ContainsKey(itemType) == false)
                g_dictOnGiveItemFunctions.Add(itemType, moveFun);
            else
                //允许Mod重写基础道具奖励
                g_dictOnGiveItemFunctions[itemType] = moveFun;
        }

        public static OnGiveItemFunction GetItemRewardFunction(int itemType)
        {
            OnGiveItemFunction ItemRewardFun;
            if (g_dictOnGiveItemFunctions.TryGetValue(itemType, out ItemRewardFun) == false)
                return null;
            return ItemRewardFun;
        }
    }
}
