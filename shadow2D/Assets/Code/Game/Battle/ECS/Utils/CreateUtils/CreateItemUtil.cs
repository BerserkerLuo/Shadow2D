
using Table;

using UnityEngine;

namespace ECS
{
    partial class CreateUtil
    {
        public static Entity CreateItem(ECSWorld world,int xmlId, Vector3 bronPos) {

            ItemBattleCfg itemBattleCfg = TableMgr.Singleton.GetItemBattleCfg(xmlId);
            if (itemBattleCfg == null)
                return null;

            Entity e = CreateEntity(world);

            InitEntityTypeComponent(e,EnumEntityType.eItem,itemBattleCfg.Id);
            InitAvatarComponent(e, itemBattleCfg.Prefab);

            e.AddComponent<AutoMoveComponent>();

            var itemComp = e.AddComponent<DropComponent>();
            itemComp.ItemId = xmlId;

            LogicUtils.SetPos(e,bronPos);

            return e;
        }
    }
}
