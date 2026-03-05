
using Table;

namespace ECS
{
    class SkillCfgUtil
    {
        public static SkillCfg GetSkillCfg(int skillId)
        {
            SkillCfg skillCfg = TableMgr.Singleton.GetSkillCfg(skillId);
            if (skillCfg == null)
                return null;
            return skillCfg; 
        }


        //是否是被动技能
        public static bool CheckSkillType(int skillid,int type){
            SkillCfg skillCfg = GetSkillCfg(skillid);
            if (skillCfg == null) return false;
            return skillCfg.SkillType == type;
        }
        public static bool IsNormalAtk(int skillId) {
            return CheckSkillType(skillId, 3);
        }

        public static string GetSkillScriptName(int skillid)
        {
            SkillCfg skillCfg = GetSkillCfg(skillid);
            if (skillCfg == null)
                return "";
            return skillCfg.ScriptName;
        }

        public static int GetSkillTargetType(int skillId)
        {
            SkillCfg skillCfg = TableMgr.Singleton.GetSkillCfg(skillId);
            if (skillCfg == null)
                return (int)SkillTargetType.Enemy;
            return skillCfg.TargetType;
        }
    }
}
