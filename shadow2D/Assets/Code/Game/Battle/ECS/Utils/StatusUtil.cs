
using Table;

namespace ECS
{
    partial class StatusUtil
    {
        public static void AddStatus(Entity le, int statusId, int layer = 1, Entity fromEntity = null)
        {
            int oldLayer = StatusDataUtils.GetLayer(le, statusId);

            int realAddlayer = StatusDataUtils.AddStatusLayer(le, statusId, layer, fromEntity);
            if (realAddlayer == 0)
                return;

            //添加状态属性
            OnAddStatusAttr(le, statusId, realAddlayer);


            if (oldLayer == 0 && realAddlayer > 0)
                AvatarDataUtil.AddStatusEffectToEntity(le, statusId);//添加表现Effect
            else if (oldLayer + realAddlayer == 0)
                RemoveStatus(le,statusId);  
        }

        public static void RemoveStatus(Entity le, int statusId)
        {
            if (StatusDataUtils.GetStatus(le, statusId) == null) 
                return;

            StatusDataUtils.DelStatus(le, statusId);

            AttrUtil.OnRemoveStatusAttr(le, statusId);

            //删除状态 State
            // to do ...

            //删除表现Effect
            AvatarDataUtil.RemoveEntityStatusEffect(le, statusId);
        }

        private static void OnAddStatusAttr(Entity e, int statusId, int addLayer)
        {
            DebugUtils.DebugLog("AddStatusAttr {}",statusId);

            StatusCfg statusCfg = TableMgr.Singleton.GetStatusCfg(statusId);
            if (statusCfg == null)
                return;

            int rowAttrType = AttrDataUtil.MakeStatusAttrRowType(statusId);

            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.BulletNum, statusCfg.BulletCount * addLayer);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.ShotNum, statusCfg.ShotNum * addLayer);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.ReloadSpeed, statusCfg.ReloadTime * addLayer);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.ShotSpeed, statusCfg.ShotSpeed * addLayer);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.BulletSpeed, statusCfg.BulletSpeed * addLayer);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.WeaponRange, statusCfg.WeaponRange * addLayer);
             
            AttrDataUtil.RefreshAttr(e);

            DebugUtils.DebugLog("AddStatusAttr {} BulletNum {}", statusId,AttrUtil.GetBulletNum(e));
        }

        public static int GetStatusLayer(Entity le, int statusId) {
            var statusInfo = StatusDataUtils.GetStatus(le, statusId);
            if (statusInfo == null)
                return -1;
            return statusInfo.StatusLayer;
        }
    }
}
