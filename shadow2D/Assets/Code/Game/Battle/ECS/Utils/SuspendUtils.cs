
using UnityEngine;

namespace ECS
{
    partial class LogicUtils
    {
        //=========================================================================================

        public static SuspendComponent GetSuspendComponent(ECSWorld logicEcsWorld)
        {
            if (logicEcsWorld.GlobalEntity == null)
                return null;

            return logicEcsWorld.GlobalEntity.GetComponentData<SuspendComponent>();
        }

        public static int AddSuspend(ECSWorld logicWorld, SuspendData data, float intervalTime, int tickCount = 1)
        {

            SuspendComponent suspendComp = GetSuspendComponent(logicWorld);
            if (suspendComp == null)
                return -1;

            if (intervalTime <0) intervalTime = 0.5f;

            data.intervalTime = intervalTime;
            data.nextTickTime = logicWorld.Time + intervalTime;
            data.residueCount = tickCount;

            int newSuspendID = suspendComp.MakeSuspendId++;
            suspendComp.AddDic.Add(newSuspendID, data);

            return newSuspendID;
        }

        public static void StopSuspend(ECSWorld logicWorld, int suspendID)
        {
            SuspendComponent suspendComp = GetSuspendComponent(logicWorld);
            if (suspendComp == null)
                return;

            suspendComp.DictSuspends.Remove(suspendID);
        }

        //=========================================================================================
        //数据结构
        private class SectorCreateBullet : SuspendData
        {
            public Entity master;
            public int bulletId;
            public float stepAngle;
            public float offsetAngle;
            public Vector3 vector;
            public int index = 0;
            public int num;
        }

        /// <summary>以某个单位为起点按扇形分布创建子弹</summary>
        /// <param name="le"        创建者    ></param> 
        /// <param name="bulletId"  子弹ID    ></param>
        /// <param name="num"       数量      ></param>
        /// <param name="angle"     扇形角度  ></param>
        /// <param name="vector"    方向和距离的向量 ></param>
        /// <param name="interval"  创建间隔 ></param>
        public static void OnCreateBulletBySector(Entity le, int bulletId, int num, float angle, Vector3 vector, float interval)
        {
            SectorCreateBullet data = new SectorCreateBullet();
            data.master = le;
            data.bulletId = bulletId;
            data.stepAngle = angle / num;
            data.offsetAngle = angle / 2 + data.stepAngle / 2;
            data.vector = vector;
            data.num = num;
            data.callBackFun = OnDelayCreateBulletBySector;
            AddSuspend(le.EcsWorld, data, interval, num);
        }

        public static void OnDelayCreateBulletBySector(ECSWorld logicWorld, SuspendData args)
        {
            SectorCreateBullet data = (SectorCreateBullet)args;

            if (IsDead(data.master))
                return;

            Vector3 bornPos = GetPos(data.master);
            Quaternion roate = Quaternion.Euler(0f, data.stepAngle * data.index + data.offsetAngle - 60, 0f);
            Vector3 newVecor = (roate * data.vector);
            Vector3 TargetPos = bornPos + newVecor;

            Entity realMaster = GetMaster(data.master);
            Entity bullet = CreateUtil.CreateBullet(logicWorld, data.bulletId, bornPos + newVecor.normalized, realMaster, newVecor.normalized);

            LogicDataUtils.SetPosMove(bullet, TargetPos);
            data.index++;
        }

    }
}
