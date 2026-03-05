
using Table;

using UnityEngine;

namespace ECS
{
    partial class CreateUtil
    {
        public static Entity CreateBullet(ECSWorld world,int xmlId, Vector3 bronPos,Entity master = null, Vector3 forward = default) {

            BulletCfg bulletData = TableMgr.Singleton.GetBulletCfg(xmlId);
            if (bulletData == null)
                return null;

            Entity e = CreateEntity(world);

            InitEntityTypeComponent(e,EnumEntityType.eBullet,bulletData.Id);
            InitAvatarComponent(e, bulletData.ModelName);
            InitBulletComponent(e, bulletData);
            InitMasterComponent(e, LogicUtils.GetMaster(master));

            e.AddComponent<AttrComponent>();
            e.AddComponent<AutoMoveComponent>();
            e.AddComponent<SkillComponent>();

            InitBulletAttr(e,bulletData);

            LogicUtils.SetPos(e,bronPos);
            LogicUtils.SetForward(e, forward);

            return e;
        }

        public static void InitBulletAttr(Entity e, BulletCfg bulletData)
        {
            AttrUtil.SetBaseAttr(e, AttrType.BodySize, bulletData.ModelScale);
            AttrUtil.SetBaseAttr(e, AttrType.MoveSpeed, bulletData.Speed);
            AttrDataUtil.RefreshAttr(e);
        }
    }
}
