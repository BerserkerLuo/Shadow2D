
using System.Collections.Generic;

namespace ECS
{
    //技能组件
    class SkillComponent : Component
    {
        //正在施法的技能
        public SkillUnit m_curCast = null;

        //可自由释放的技能
        public Dictionary<int, SkillUnit> m_freeCastSkillDic = new Dictionary<int, SkillUnit>();
        public List<int> m_endList = new List<int>();

        //技能CD数据 skillId => SkillCDUnit
        public Dictionary<int, SkillCDUnit> m_dicID2CD = new Dictionary<int, SkillCDUnit>();

        //技能等级数据 skillId => level
        public Dictionary<int, SkillInfo> SkillInfos = new Dictionary<int, SkillInfo>();
        public int BaseNormalAtkId = 0;     //普攻ID
        public int WeaponNormalAtkId = 0;   //武器普攻ID 
    }
}
