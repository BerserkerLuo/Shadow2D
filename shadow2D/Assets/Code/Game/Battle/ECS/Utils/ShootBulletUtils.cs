using System.Collections.Generic;
using Table;
using UnityEngine;
namespace ECS
{
    public class ShootBulletUtils
    {

        //1,直射
        //2,抛物线
        //3,贝塞尔跟踪曲线

        private class SuspendData_ShootSkillBullet : SuspendData
        {
            public Entity master;
            public SkillCfg skillCfg;
            public SkillLvCfg skillLvCfg;
            public Vector2 targetPos;
        }

        public static void TryShootSkillBullet(Entity master, SkillUnit skillUnit, Vector2 targetPos) {

            if (skillUnit.skillLvCfg.BulletID == 0)
                return;

            if (ScriptUtils.OnShootBullet(master, skillUnit, targetPos))
                return;

            SuspendData_ShootSkillBullet data = new SuspendData_ShootSkillBullet();
            data.master = master;
            data.skillCfg = skillUnit.skillCfg;
            data.skillLvCfg = skillUnit.skillLvCfg;
            data.targetPos = targetPos;
            data.callBackFun = ShootSkillBullet;
            LogicUtils.AddSuspend(master.EcsWorld,data,0.1f, 1);
        }

        private static void ShootSkillBullet(ECSWorld world, SuspendData s) {
            SuspendData_ShootSkillBullet data = (SuspendData_ShootSkillBullet)s;
            if (LogicUtils.IsDead(data.master))
                return;

            int bulletCount = (int)AttrUtil.GetShotNum(data.master) + data.skillLvCfg.ShotNum;

            switch (data.skillLvCfg.ShootType) {
                case 2:{
                        for (int i = 0; i < bulletCount; ++i)
                            ShootBulletByParabola(data.skillLvCfg.BulletID, data.targetPos, data.master);
                    }break;
                case 3:{
                    for (int i = 0; i < bulletCount; ++i)
                        ShootBulletByBezier(data.skillLvCfg.BulletID, data.targetPos, data.master);
                    }break;
                default:
                    ShootBulletByDirect(data.skillLvCfg.BulletID, data.targetPos - LogicUtils.GetPos(data.master), data.master);
                    break;
            }
        }

        //直射
        private static void ShootBulletByDirect(int bulletId,Vector3 dire,Entity master) {

            float direAngle = Mathf.Atan2(dire.y, dire.x) * 180 / Mathf.PI;

            Vector3 srcPos = LogicUtils.GetPos(master);
            Entity bullet = CreateUtil.CreateBullet(master.EcsWorld, bulletId, srcPos, master);
            LogicDataUtils.SetPosMove(bullet, srcPos+(dire.normalized * 50));
        }

        //抛物线
        private static void ShootBulletByParabola(int bulletId, Vector3 tarPos, Entity master)
        {
            Vector3 srcPos = LogicUtils.GetPos(master);

            tarPos.x += LogicUtils.GetRand(-0.2f, 0.2f);
            tarPos.y += LogicUtils.GetRand(-0.2f, 0.2f);

            Vector2 midPos = Vector2.Lerp(srcPos, tarPos, 0.5f);
            midPos.y += LogicUtils.GetRand(3, 6);

            List<Vector2> path = LogicUtils.GetBezierPath20(srcPos, midPos, tarPos);

            Entity bullet = CreateUtil.CreateBullet(master.EcsWorld, bulletId, srcPos, master);

            LogicDataUtils.SetPathMove(bullet, path);
        }

        //贝塞尔路线
        private static void ShootBulletByBezier(int bulletId, Vector3 tarPos, Entity master) {
            //tarPos.x += LogicUtils.GetRand(-0.2f, 0.2f);
            //tarPos.y += LogicUtils.GetRand(-0.2f, 0.2f);

            Vector3 srcPos = LogicUtils.GetPos(master);

            Vector2 midPos = Vector2.Lerp(srcPos, tarPos, LogicUtils.GetRand(0.2f, 0.5f));

            Vector2 tarVec = (tarPos - srcPos);
            float disance = tarVec.magnitude;
            Vector2 dire2 = new Vector2(tarVec.y, -tarVec.x).normalized;

            Vector2 c1 = midPos + dire2 * disance * LogicUtils.GetRand(0.2f,0.6f);
            Vector2 c2 = midPos - dire2 * disance * LogicUtils.GetRand(0.2f,0.6f);

            List<Vector2> path = null;
            if(LogicUtils.GetRand(0,100) > 50)
                path = LogicUtils.GetBezierPath20(srcPos, c1, c2, tarPos);
            else
                path = LogicUtils.GetBezierPath20(srcPos, c2, c1, tarPos);

            Entity bullet = CreateUtil.CreateBullet(master.EcsWorld, bulletId, srcPos, master);
            LogicDataUtils.SetPathMove(bullet, path);

            //DebugMgr.UpdateTargetPos(bullet, path[path.Count-1]);
        }


        //Old
        //private static void DelayCreateBulletToPos(ECSWorld ecsWorld, SuspendData s)
        //{
        //    SuspendData_CreateBulletToPos data = (SuspendData_CreateBulletToPos)s;

        //    Vector3 srcPos = LogicUtils.GetPos(data.master);
        //    Vector3 stepPos = (data.TargetPos - srcPos).normalized * 0.5f;

        //    //DebugUtils.DebugLog("Begin srcPos{} stepPos{}", srcPos.ToString(), stepPos);

        //    for (int i = 0; i < data.count; ++i)
        //    {

        //        Vector3 BronPos = srcPos;
        //        if (i != 0)
        //        {
        //            int loopCount = (i + 1) / 2;
        //            if (i % 2 == 0)
        //                BronPos = new Vector3(srcPos.x - stepPos.y * loopCount, srcPos.y + stepPos.x * loopCount, 0);
        //            else
        //                BronPos = new Vector3(srcPos.x + stepPos.y * loopCount, srcPos.y - stepPos.x * loopCount, 0);
        //        }

        //        //DebugUtils.DebugLog("BronPos {} loopCount", BronPos.ToString());

        //        Entity bullet = CreateUtil.CreateBullet(ecsWorld, data.bulletId, BronPos, data.master);
        //        LogicDataUtils.SetPosMove(bullet, data.TargetPos);
        //    }
        //}
    }
}