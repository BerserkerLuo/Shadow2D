
using System.Collections.Generic;

namespace ECS
{
    public class AttrInfo
    {
        public static AttrInfo operator +(AttrInfo lhs, AttrInfo rhs){
            lhs.value += rhs.value;
            lhs.percent += rhs.percent;
            return lhs;
        }

        public static AttrInfo operator +(AttrInfo lhs, float rhs){
            lhs.value += rhs;
            return lhs;
        }

        public void Clear(){
            value = 0f;
            percent = 0f;
        }

        public float value = 0f;
        public float percent = 0f;
    }

    public class AttrRow
    {

        public AttrInfo GetAttrInfo(int attrType){
            AttrInfo retInfo = attrs.GetValueOrDefault(attrType, null);
            if (retInfo == null) {
                retInfo = new AttrInfo();
                attrs.Add(attrType, retInfo);
            }
            return retInfo;
        }

        public void clear(){
            attrs.Clear();
        }

        public Dictionary<int, AttrInfo> attrs = new Dictionary<int, AttrInfo>();
        public bool isValid = true;
    }

    //单位战斗属性
    public class AttrComponent : Component
    {
        public bool IsChange = false;
        public HashSet<int> ChangeAttrTypeList = new HashSet<int>();

        public Dictionary<int, AttrRow> AttrMap { get { return m_AttrMap; } }
        public Dictionary<int, float> FinalAttr { get { return m_dictFinalAttr; } set { m_dictFinalAttr = value; } }

        private Dictionary<int, AttrRow> m_AttrMap = new Dictionary<int, AttrRow>();
        private Dictionary<int, float> m_dictFinalAttr = new Dictionary<int, float>();
    }
}
