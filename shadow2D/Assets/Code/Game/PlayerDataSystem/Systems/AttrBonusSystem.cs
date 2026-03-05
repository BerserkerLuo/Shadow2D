
using System.Collections.Generic;
using System.Linq;

namespace PlayerSystemData
{
    public class AttrBonusSystem : BaseDataSystem
    {
        #region
        public static AttrBonusSystem Singleton
        {
            get
            {
                if (null == s_singleton) s_singleton = new AttrBonusSystem();
                return s_singleton;
            }
        }
        static AttrBonusSystem s_singleton;
        #endregion

        AttrLevelData attrData = null;
        public override string SystemName { get { return "AttrBonusSystem"; } }
        public AttrBonusSystem()
        {
            attrData = new AttrLevelData();
            systemData = attrData;
        }

        public int GetAttrLevel(int attrType)
        {
            var item = GetAttrInfo(attrType);
            if (item == null)
                return 0;

            return item.attrLevel;
        }

        //获取道具
        public AttrLevelInfo GetAttrInfo(int attrType)
        {
            AttrLevelInfo info = null;
            bool exist =  attrData.attrs.TryGetValue(attrType, out info);
            return info;
        }

        //获取所有场外附加属性信息
        public List<AttrLevelInfo> GetAttrInfoList()
        {
            return attrData.attrs.Values.ToList();
        }

        public bool TryChangeAttrLevel(int attrType, int v)
        {
            AttrLevelInfo item = GetAttrInfo(attrType);
            if (item == null)
            {
                item = new AttrLevelInfo();
                item.attrType = attrType;
            }

            var tarLevel = v;
            if (tarLevel < 0)
                return false;

            item.attrLevel = tarLevel;
            attrData.attrs[attrType] = item;

            if (item.attrLevel == 0)
                attrData.attrs.Remove(attrType);
            SystemUtils.SetDataDirty();
            return true;
        }
    }
}
