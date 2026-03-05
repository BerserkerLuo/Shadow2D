
using ECS.Script;
using Table;

namespace ECS
{
    partial class ScriptUtils
    {

        public static ScriptBase GetNewStatusScript(int statusId)
        {
            StatusCfg statusCfg = TableMgr.Singleton.GetStatusCfg(statusId);
            if (statusCfg == null || statusCfg.Script == "")
                return ScriptMgr.DefaultScript;

            return ScriptMgr.Singleton.GetNewScript(statusCfg.Script);
        }

        public static float OnStatusPostTarget(Entity src,Entity tar,int skillId,float dmage) {
            return 0;
        }

        public static float OnStatusPostSource(Entity src, Entity tar, int skillId, float dmage){
            return 0;
        }
    }
}
