

using System.Collections.Generic;
using Table;

namespace ECS
{
    class AttrUtil
    {
        //=====================================================================================================

        public static float GetAttr(Entity e,int attrType) {return AttrDataUtil.GetFinalAttr(e, attrType);}

        public static float GetHP(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.HP); }
        public static float GetHPMax(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.HPMax); }
        public static float GetAtk(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.Atk); }
        public static float GetCrit(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.Crit); }
        public static float GetCritEffect(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.CritEffect); }
        //public static float GetDef(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.Def); }
        public static float GetSpeed(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.MoveSpeed); }
        public static float GetCdReduce(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.ReloadSpeed); }
        public static float GetShotNum(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.ShotNum); }
        public static float GetBulletNum(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.BulletNum); }
        //public static float GetBulletPenet(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.BulletPenet); }
        //public static float GetBulletCatapult(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.BulletCatapult); }
        public static float GetBodySize(Entity e) { return AttrDataUtil.GetFinalAttr(e, AttrType.BodySize); }

        //=================================================================================================
        private static void SetAttr(Entity e, int rowAttrType, int attrType, float attrValue, float attrPercent)
        {
            AttrDataUtil.SetAttr(e, rowAttrType, attrType, attrValue, attrPercent);
            AttrDataUtil.RefreshAttr(e);
        }

        private static void AddAttr(Entity e, int rowAttrType, int attrType, float attrValue, float attrPercent)
        {
            AttrDataUtil.AddAttr(e, rowAttrType, attrType, attrValue, attrPercent);
            AttrDataUtil.RefreshAttr(e);
        }

        //=====================================================================================================
        //Base 基础属性

        public static int BaseAttrRowID = AttrDataUtil.MakeBaseAttrRowType();
        public static int WeaponAttrRowID = AttrDataUtil.MakeWeaponAttrRowType();
        public static void SetBaseAttr(Entity e, int attrType, float attrValue, float attrPercent = default)
        {
            AttrDataUtil.SetAttr(e, BaseAttrRowID, attrType, attrValue, attrPercent);
        }

        public static void AddBaseAttr(Entity e, int attrType, float attrValue, float attrPercent = default)
        {
            AttrDataUtil.AddAttr(e, BaseAttrRowID, attrType, attrValue, attrPercent);
        }
        public static void AddBaseAttr(Entity e, Dictionary<string, float> Attr, Dictionary<string, float> AttrPer)
        {
            AttrDataUtil.AddAttr(e, BaseAttrRowID, Attr, AttrPer);
        }
        //=====================================================================================================

        public static void OnRemoveEquipAttr(Entity e, int equipId) {
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            if (ac == null)
                return;

            int rowAttrType = AttrDataUtil.MakeEquipAttrRowType(equipId);
            AttrDataUtil.RemoveAttrRow(e, rowAttrType);
            AttrDataUtil.RefreshAttr(e);
        }
        public static void AddEquipAttr(Entity e, int equipId,ItemCfg itemCfg){
            int rowAttrType = AttrDataUtil.MakeEquipAttrRowType(equipId);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.Str, itemCfg.Str);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.Int, itemCfg.Int);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.Agi, itemCfg.Agi);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.Phy, itemCfg.Phy);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.Spi, itemCfg.Spi);
            AttrDataUtil.AddAttr(e, rowAttrType, AttrType.Luck, itemCfg.Luck);
            AttrDataUtil.RefreshAttr(e);
        }

        //=====================================================================================================
        //Status  状态

        public static void OnRemoveStatusAttr(Entity e, int statusId)
        {
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            if (ac == null)
                return;

            int rowAttrType = AttrDataUtil.MakeStatusAttrRowType(statusId);
            AttrDataUtil.RemoveAttrRow(e, rowAttrType);
            AttrDataUtil.RefreshAttr(e);
        }

        public static void AddStatusAttr(Entity e, int statusId, int attrType, float attrValue, float attrPercent = default)
        {
            AddAttr(e, AttrDataUtil.MakeStatusAttrRowType(statusId), attrType, attrValue, attrPercent);
        }

        //=====================================================================================================

        private static float ChangeLimitedAttr(Entity e, float changeValue, int maxAttrType, int curAttrType)
        {
            float maxValue = AttrDataUtil.GetFinalAttr(e, maxAttrType);
            float curValue = AttrDataUtil.GetFinalAttr(e, curAttrType);
            float newValue = curValue + changeValue;


            if (newValue > maxValue) newValue = maxValue;
            if (newValue < 0) newValue = 0;

            float realChangeValue = newValue - curValue;

            AttrDataUtil.SetFinalAttr(e, curAttrType, newValue);

            return realChangeValue;
        }

        public static float ChangeHP(Entity e, float addValue)
        {
            float realChangeValue = ChangeLimitedAttr(e, addValue, AttrType.HPMax, AttrType.HP);
            if(realChangeValue > 0.01f || realChangeValue < -0.01f)
                UIUtils.OnHPChange(e);
            return realChangeValue;
        }
    }
}
