using Client.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Table;
using Tool;

namespace ECS
{
    public class SkillRandUtils
    {
        public static DropGroup GetSkillDropGroup(Entity e) {
            int dropId = GlobalCfg.GetWeaponShopDropId();

            DropGroup dropGroup = DropUtils.GetDropGroup(dropId);
            DropGroup skillGroup = new DropGroup();
            foreach (var info in dropGroup.infos)
            {
                ItemBattleCfg itemcfg = TableMgr.Singleton.GetItemBattleCfg(info.dropCfg.ItemId);
                if (itemcfg == null)
                    continue;

                int skillId = itemcfg.Value1;
                int skillLevel = SkillDataUtil.GetRealSkillLevel(e, skillId);
                SkillLvCfg skillLvCfg = TableMgr.Singleton.GetSkillLvCfg(skillId, skillLevel + 1);
                if (skillLvCfg == null)
                    continue;

                DropInfo newInfo = info.Clone();
                if (skillLevel > 0)
                    newInfo.weight *= 1.2f;

                newInfo.p1 = itemcfg.ItemType;
                newInfo.p2 = skillLvCfg.Level;

                skillGroup.AddInfo(newInfo);
            }
            return skillGroup;
        }

        public static DropGroup GetStatusDropGroup(Entity e)
        {
            int dropId = GlobalCfg.GetLevelUpDropId();

            DropGroup dropGroup = DropUtils.GetDropGroup(dropId);
            DropGroup statusGroup = new DropGroup();
            foreach (var info in dropGroup.infos)
            {
                ItemBattleCfg itemcfg = TableMgr.Singleton.GetItemBattleCfg(info.dropCfg.ItemId);
                if (itemcfg == null)
                    continue;

                int statusId = itemcfg.Value1;
                int statusLayer = StatusDataUtils.GetLayer(e, statusId);

                StatusCfg statusCfg = TableMgr.Singleton.GetStatusCfg(statusId);
                if (statusCfg == null || statusCfg.MaxLayer <= statusLayer)
                    continue;

                DropInfo newInfo = info.Clone();
                if (statusLayer > 0)
                    newInfo.weight *= 1.2f;

                newInfo.p1 = itemcfg.ItemType;
                newInfo.p2 = statusLayer + 1;

                statusGroup.AddInfo(newInfo);
            }
            return statusGroup;
        }

        //=================================================================================

        public static List<ShopItemData> DropGroup(DropGroup dropGroup,int times,bool canRepeat) {
            List<ShopItemData> retList = new List<ShopItemData>();

            for (int i = 0; i < times; ++i){
                int index = DropUtils.ExecuteDrop(dropGroup.infos, dropGroup.totalWeight);
                if (index == -1)
                    continue;
                DropInfo dropInfo = dropGroup.infos[index];

                if (!canRepeat) {
                    dropGroup.infos.RemoveAt(index);
                    dropGroup.totalWeight -= dropInfo.weight;
                }

                ShopItemData shopItem = new ShopItemData();
                shopItem.ItemId = dropInfo.dropCfg.ItemId;
                shopItem.Level = dropInfo.p2;
                retList.Add(shopItem);
            }
            return retList;
        }

        public static List<ShopItemData> RandSkillList(Entity e) {
            DropGroup dropGroup = GetSkillDropGroup(e);
            return DropGroup(dropGroup,3,false);
        }

        public static List<ShopItemData> RandStatusList(Entity e){
            DropGroup dropGroup = GetStatusDropGroup(e);
            return DropGroup(dropGroup, 3, false);
        }

    }
}