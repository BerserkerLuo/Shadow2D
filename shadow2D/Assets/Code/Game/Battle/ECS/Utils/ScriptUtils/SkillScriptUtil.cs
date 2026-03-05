
using ECS.Script;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    //技能脚本入口 
    //用静态还是用单例 似乎都可以啊
    partial class ScriptUtils
    {

        //获取技能脚本
        public static ScriptBase GetSkillScript(int skillId)
        {
            string scriptName = SkillCfgUtil.GetSkillScriptName(skillId);
            if (scriptName == null || scriptName == "")
                return null;

            ScriptBase script = ScriptMgr.Singleton.GetScript(scriptName);

            return script;
        }

        //检测技能是否可以释放
        public static bool CheckSkillCanCast(Entity e, int skillId)
        {
            ScriptBase script = GetSkillScript(skillId);
            if (script != null)
            {
                return script.OnSkillCheckCanCast(e, skillId);
            }

            return true;
        }

        //开始施法的回调
        public static void OnSkillStart(SkillUnit skillUnit, Entity e)
        {
            ScriptBase script = GetSkillScript(skillUnit.skillId);
            if (script == null)
                return;

            script.OnSkillStart(e, skillUnit);
        }

        //开始引导
        public static void OnSkillChannelBegin(SkillUnit skillUnit, Entity e)
        {
            ScriptBase script = GetSkillScript(skillUnit.skillId);
            if (script == null)
                return;
            script.OnSkillChannelBegin(e, skillUnit);
        }

        //开始射击 出手
        public static void OnSkillShotBegin(SkillUnit skillUnit, Entity e)
        {
            ScriptBase script = GetSkillScript(skillUnit.skillId);
            if (script == null)
                return;

            script.OnSkillShotBegin(e, skillUnit);
        }

        //技能完成 开始
        public static void OnSkillFinishBegin(SkillUnit skillUnit, Entity e)
        {
            ScriptBase script = GetSkillScript(skillUnit.skillId);
            if (script == null)
                return;

            script.OnSkillFinishBegin(e, skillUnit);
        }

        //技能彻底结束
        public static void OnSkillFinishEnd(SkillUnit skillUnit, Entity e)
        {
            ScriptBase script = GetSkillScript(skillUnit.skillId);
            if (script == null)
                return;
            script.OnSkillFinishEnd(e, skillUnit);
        }

        //技能攻击玩家
        public static void OnSkillAttackTarget(Entity e, SkillUnit skillUnit, Entity tarEntity)
        {
            ScriptBase skillScript = GetSkillScript(skillUnit.skillId);
            if (skillScript == null)
                return;
            skillScript.OnSkillAttackTarget(e, skillUnit, tarEntity);
        }

        public static bool OnShootBullet(Entity e, SkillUnit skillUnit, Vector2 tarPos) {
            ScriptBase skillScript = GetSkillScript(skillUnit.skillId);
            if (skillScript == null)
                return false;
            return skillScript.OnShootBullet(e,skillUnit, tarPos);
        }
    }
}
