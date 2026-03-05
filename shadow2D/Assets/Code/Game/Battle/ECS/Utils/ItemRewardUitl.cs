using System.Collections;
using UnityEngine;
using Table;

namespace ECS
{
    public class ItemRewardUitl
    {
        //拾取道具
        public static void OnPickUpItem(Entity le, Entity item){
            var itemComp = item.GetComponentData<DropComponent>();
            if (itemComp == null)
                return;

            OnGiveItem(le, itemComp.ItemId,1);
            LogicUtils.KillEntity(item);
        }

        public static void OnGiveItem(Entity e,int itemId,int count) {
            if (count == 0)
                return;

            ItemBattleCfg itemcfg = TableMgr.Singleton.GetItemBattleCfg(itemId);
            if (itemcfg == null)
                return;

            var GiveItemFun = ItemRewardDispatcher.GetItemRewardFunction(itemcfg.ItemType);
            if (GiveItemFun == null)
                return;

            Debug.Log($"OnGiveItem itemId {itemId}");

            GiveItemFun(e,itemcfg, count);
        }

        public static void OnAddExp(Entity e, ItemBattleCfg itemcfg, int count) {
            var comp = e.GetComponentData<ExpComponent>();
            if (comp == null)
                return;

            Debug.Log($"OnAddExp {count} {itemcfg.Value1}");

            comp.exp += itemcfg.Value1 * count;

            int addlevel = (int)(comp.exp / comp.lvUpCost);
            comp.exp = comp.exp - addlevel * comp.lvUpCost;

            UIUtils.OnRefreshExp();

            if (addlevel != 0)
                UIUtils.OnShowLevelUp(addlevel);
        }

        public static void OnAddGold(Entity e, ItemBattleCfg itemcfg, int count){
            int addCount = itemcfg.Value1* count;
            BackpackUtil.AddIntoBackpack(e,ItemId.CoinId, addCount);
            UIUtils.OnRefreshCoin(addCount);
        }

        public static void OnAddSkill(Entity e, ItemBattleCfg itemcfg, int count){
            SkillUtils.AddSkill(e, itemcfg.Value1,count);
        }

        public static void OnAddStatus(Entity e, ItemBattleCfg itemcfg, int count) {
            StatusUtil.AddStatus(e, itemcfg.Value1, count);
        }

        public static void OnUseSkill(Entity e, ItemBattleCfg itemcfg, int count){
            SkillUtils.CastSKillToTargetBySkillId(e, itemcfg.Value1, e.Eid);
        }

    }
}