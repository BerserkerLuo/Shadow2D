using ECS.Script;
using Table;


namespace ECS
{
    partial class ScriptUtils
    {

        public static ScriptBase GetBulletScript(int bulletCfgId)
        {
            BulletCfg bulletCfg = TableMgr.Singleton.GetBulletCfg(bulletCfgId);
            if (bulletCfg == null)
                return null;
            if (bulletCfg.ScriptName == "")
                return null;

            return ScriptMgr.Singleton.GetScript(bulletCfg.ScriptName);
        }

        //子弹碰到敌人
        public static void OnBulletCollide(int bulletCfgId, Entity bullet, Entity tarEntity)
        {
            ScriptBase skillScript = GetBulletScript(bulletCfgId);
            if (skillScript == null)
                return;
            skillScript.OnBulletCollide(bullet,tarEntity);
        }

        //子弹到达目标点
        public static void OnBulletArriveDestination(int bulletCfgId, Entity bullet)
        {
            ScriptBase skillScript = GetBulletScript(bulletCfgId);
            if (skillScript == null)
                return;
            skillScript.OnBulletArriveDestination(bullet);
        }
        
    }
}
