
using System.Collections.Generic;
using Table;
using Unity.VisualScripting;

namespace ECS
{
    public class AttrDataUtil
    {
        private static Dictionary<int, int> m_limitAttrMap = null;
        private static Dictionary<int, int> LimitAttrMap{
            get{
                if (m_limitAttrMap == null){
                    m_limitAttrMap = new Dictionary<int, int>();

                    //关联属性
                    m_limitAttrMap.Add(AttrType.HP, AttrType.HPMax);
                    //m_limitAttrMap.Add(AttrType.MP, AttrType.MPMax);
                }
                return m_limitAttrMap;
            }
        }

        //=====================================================================================================
        //生成属性列ID
        public static int MakeAttrRowType(int id, int endId) { return id << 4 | endId; }

        //基础属性
        public static int MakeBaseAttrRowType() { return MakeAttrRowType(1, 0); }

        //1级属性转换后属性
        public static int MakeConvAttrRowType() { return MakeAttrRowType(2, 0); }

        //武器属性
        public static int MakeWeaponAttrRowType() { return MakeAttrRowType(3, 0); }

        //生成状态属性列Type
        public static int MakeStatusAttrRowType(int statusId) { return MakeAttrRowType(statusId, 1); }
        //生成装备属性列Type
        public static int MakeEquipAttrRowType(int equipId) { return MakeAttrRowType(equipId, 2); }

        //=====================================================================================================
        //AttrRow 属性列

        public static AttrRow GetAttrRow(Entity e, int rowType){return GetAttrRow(e.GetComponentData<AttrComponent>(),rowType);}
        public static AttrRow GetAttrRow(AttrComponent ac, int rowType) {
            var AttrMap = ac.AttrMap;
            if (!AttrMap.ContainsKey(rowType))
                AttrMap.Add(rowType, new AttrRow());

            return AttrMap[rowType];
        }

        public static void RemoveAttrRow(Entity e, int rowType) {RemoveAttrRow(e.GetComponentData<AttrComponent>(), rowType);}
        public static void RemoveAttrRow(AttrComponent ac, int rowType) {
            AttrRow attrRow = ac.AttrMap.GetValueOrDefault(rowType,null);
            if (attrRow == null)
                return;

            ac.ChangeAttrTypeList.UnionWith(attrRow.attrs.Keys);
            ac.AttrMap.Remove(rowType);
            SetAttrChange(ac);
        }

        //=====================================================================================================
        //AttrInfo 属性信息

        public static AttrInfo GetAttrInfo(Entity e, int rowType, int attrType){
            return GetAttrRow(e, rowType).GetAttrInfo(attrType);
        }
        public static AttrInfo GetAttrInfo(AttrComponent ac, int rowType, int attrType){
           return GetAttrRow(ac, rowType).GetAttrInfo(attrType);
        }

        public static void SetAttr(Entity e, int rowType, int attrType, float attrValue, float attrPer = default) {
            AttrInfo attrInfo = GetAttrInfo(e, rowType, attrType);
            attrInfo.value = attrValue;
            attrInfo.percent = attrPer;
            SetAttrChange(e, attrType);
        }

        public static void AddAttr(Entity e, int rowType, int attrType, float attrValue, float attrPer = default)
        {
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            if (ac == null)
                return;
            AddAttr(ac, rowType, attrType, attrValue, attrPer);
        }

        public static void AddAttr(AttrComponent ac, int rowType, int attrType, float attrValue, float attrPer = default)
        {
            AttrInfo attrInfo = GetAttrInfo(ac, rowType, attrType);
            attrInfo.value += attrValue;
            attrInfo.percent += attrPer;

            SetAttrChange(ac, attrType);
        }

        public static void AddAttr(Entity e, int rowType, Dictionary<string, float> Attr, Dictionary<string, float> AttrPer)
        {
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            if (ac == null)
                return;

            AddAttr(ac, rowType, Attr, AttrPer);
        }

        public static void AddAttr(AttrComponent ac, int rowType, Dictionary<string, float> Attr, Dictionary<string, float> AttrPer) {
            if (Attr != null){
                foreach (var it in Attr){
                    int attrType = AttrType.GetAttrType(it.Key);
                    AttrInfo attrInfo = GetAttrInfo(ac, rowType, attrType);
                    attrInfo.value += it.Value;
                    SetAttrChange(ac, attrType);
                }
            }

            if (AttrPer != null){
                foreach (var it in AttrPer){
                    int attrType = AttrType.GetAttrType(it.Key);
                    AttrInfo attrInfo = GetAttrInfo(ac, rowType, attrType);
                    attrInfo.percent += it.Value;
                    SetAttrChange(ac, attrType);
                }
            }
        }

        //=====================================================================================================
        //属性刷新

        public static void SetAttrChange(Entity e, int attrType)
        {
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            SetAttrChange(ac, attrType);
        }

        public static void SetAttrChange(AttrComponent ac, int attrType) {
            ac.IsChange = true;
            ac.ChangeAttrTypeList.Add(attrType);
        }


        public static void SetAttrChange(Entity e){
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            SetAttrChange(ac);
        }
        public static void SetAttrChange(AttrComponent ac){
            ac.IsChange = true;
        }

        //汇总属性
        public static AttrRow CalcFinalAttrRow = new AttrRow();
        public static void SummaryAttr(Dictionary<int, float> SummaryMap, AttrComponent attrComp,int begin,int end)
        {
            CalcFinalAttrRow.clear();

            //汇总属性
            foreach (var t in attrComp.AttrMap)
            {
                AttrRow attrRow = t.Value;
                if (!attrRow.isValid)
                    continue;

                foreach (var attrIt in attrRow.attrs)
                {
                    int attrKey = attrIt.Key;
                    if (!attrComp.ChangeAttrTypeList.Contains(attrKey))
                        continue;

                    if (attrKey < begin || attrKey >= end)
                        continue;

                    AttrInfo attrInfo = CalcFinalAttrRow.GetAttrInfo(attrIt.Key);
                    attrInfo += attrRow.attrs[attrIt.Key];
                }
            }

            //结算属性
            foreach (var attrIt in CalcFinalAttrRow.attrs)
                SummaryMap[attrIt.Key] = attrIt.Value.value * (1 + attrIt.Value.percent);
        }

        //一级属性转二级属性
        public static void Lv1AttrToLv2Attr(Dictionary<int, float> SummaryMap, AttrComponent attrComp) {
            int rowType = MakeConvAttrRowType();
            AttrRow attrRow = GetAttrRow(attrComp, rowType);
            attrRow.clear();

            foreach (var it in SummaryMap) {
                AttrConvertCfg cfg = TableMgr.Singleton.GetAttrConvertCfg(it.Key);
                if (cfg == null)
                    continue;

                foreach (var attrit in cfg.ConvertMap) {
                    int attrType = AttrType.GetAttrType(attrit.Key);
                    if (attrType == -1)
                        continue;
                    AttrInfo info = attrRow.GetAttrInfo(attrType);
                    info += attrit.Value * it.Value;
                    attrComp.ChangeAttrTypeList.Add(attrType);
                }
            }
        }

        //重新计算血量等有上限的属性值
        public static void ClacLimitAttr(Dictionary<int, float> newAttr, Dictionary<int, float> oldAttr)
        {
            foreach (var it in LimitAttrMap)
            {
                int attrMaxType = it.Value;

                if (newAttr.ContainsKey(attrMaxType) == false)
                    continue;

                int attrType = it.Key;
                float attr = _GetFinalAttr(oldAttr, attrType);
                float maxAttr = _GetFinalAttr(oldAttr, attrMaxType);

                float newPer = 1;
                if (maxAttr > 0)
                    newPer = attr / maxAttr;

                float newMaxAttr = _GetFinalAttr(newAttr, attrMaxType);
                _SetFinalAttr(newAttr, attrType, newMaxAttr * newPer);
            }
        }

        //刷新最终属性
        public static void RefreshFinalAttr(Dictionary<int, float> newFinalAttr, Dictionary<int, float> target)
        {
            foreach (var it in newFinalAttr)
            {
                if (target.ContainsKey(it.Key) == false)
                    target.Add(it.Key, it.Value);
                else
                    target[it.Key] = it.Value;
            }
        }

        public static Dictionary<int, float> NewFinalAttr = new Dictionary<int, float>();
        //结算1级属性
        //结算1级属性的增加值
        //结算2级属性
        public static void RefreshAttr(Entity e)
        {
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            if (ac == null)
                return;

            if (!ac.IsChange)
                return;

            RefreshAttr(ac);
        }

        public static void RefreshAttr(AttrComponent ac) {
            NewFinalAttr.Clear();

            //累加1级属性
            //SummaryAttr(NewFinalAttr, ac, AttrType.Lv1Begin, AttrType.Lv1End);
            ////结算1级属性加成
            //Lv1AttrToLv2Attr(NewFinalAttr, ac);
            //累加二级属性
            SummaryAttr(NewFinalAttr, ac, AttrType.Lv2Begin, AttrType.Lv2End);
            //计算限制值
            ClacLimitAttr(NewFinalAttr, ac.FinalAttr);
            //刷新最终属性
            RefreshFinalAttr(NewFinalAttr, ac.FinalAttr);

            ac.ChangeAttrTypeList.Clear();
        }

        //=====================================================================================================
        //最终属性

        private static float _GetFinalAttr(Dictionary<int, float> finalAttr, int attrType)
        {
            if (!finalAttr.ContainsKey(attrType))
                return 0;
            return finalAttr[attrType];
        }

        private static void _SetFinalAttr(Dictionary<int, float> finalAttr, int attrType, float attrValue)
        {
            if (!finalAttr.ContainsKey(attrType))
                finalAttr.Add(attrType, attrValue);
            else
                finalAttr[attrType] = attrValue;
        }

        public static float GetFinalAttr(Entity e, int attrType)
        {
            if (e == null)
                return 0;

            AttrComponent ac = e.GetComponentData<AttrComponent>();
            if (ac == null)
                return 0;

            return _GetFinalAttr(ac.FinalAttr, attrType);
        }

        public static float GetFinalAttr(AttrComponent ac, int attrType){
            return _GetFinalAttr(ac.FinalAttr, attrType);
        }

        public static void SetFinalAttr(Entity e, int attrType, float attrValue)
        {
            AttrComponent ac = e.GetComponentData<AttrComponent>();
            if (ac == null)
                return;

            _SetFinalAttr(ac.FinalAttr, attrType, attrValue);
        }

    }
}
