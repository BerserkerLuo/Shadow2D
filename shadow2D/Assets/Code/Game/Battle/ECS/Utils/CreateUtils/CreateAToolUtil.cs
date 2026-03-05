
using Table;
using UnityEngine;

namespace ECS
{
    partial class CreateUtil
    {
        //创建一个Entity
        public static Entity CreateEntity(ECSWorld logicWorld)
        {
            var e = new Entity(logicWorld);
            e.Eid = logicWorld.CreateNewEntityId();
            logicWorld.OnCreateEntity(e);

            e.AddComponent<LockerComponent>();

            return e;
        }
         
        //初始类型组件
        private static void InitEntityTypeComponent(Entity e, int type,int xmlId)
        {
            var comp = e.AddComponent<BasicComponent>();
            comp.Type = type;
            comp.XmlId = xmlId;
        }

        //初始化Avatar组件
        private static void InitAvatarComponent(Entity e, string modelName)
        {
            e.AddComponent<AvatarComponent>();
            AvatarDataUtil.AddMainModelToEntity(e, ECSModelObject.GetByModelName(modelName));
        }

        //初始化子弹组件
        private static void InitBulletComponent(Entity e, BulletCfg bulletData) {
            var comp = e.AddComponent<BulletComponent>();
            comp.CollideCount = bulletData.CollideCount;
            comp.BulletCfgID = bulletData.Id;
            comp.IsCollider = true;
            comp.IsEndPosExplode = true;

            if(bulletData.LifeTime > 0)
                comp.EndLifeTime = LogicUtils.GetTime(e) + bulletData.LifeTime;
        }

        //初始化主人组件
        private static void InitMasterComponent(Entity e,Entity Master) {
            var comp = e.AddComponent<MasterComponent>();
            comp.Master = Master;
        } 

        //初始化RVO组件
        private static void InitRVOComonent(Entity e,Vector3 bronPos,float bodySize,float agentTime,int layer = 0) {
            int agentNo = RVOUtil.AddAgent(bronPos, bodySize, agentTime,layer);
            RVOComponent rvoComponent = e.AddComponent<RVOComponent>();
            rvoComponent.AgentNo = agentNo;
        }

        //初始化阵营组件
        private static void InitFactionComponent(Entity e,int factionId) {
            var comp = e.AddComponent<FactionComponent>();
            comp.FactionId = factionId;
        }

        //初始化存在时间组件
        private static void InitLifeTimeComponent(Entity e,float lifeTime) {
            var comp = e.AddComponent<LifeTimeComponent>();
            comp.DeadTime = LogicUtils.GetTime(e) + lifeTime;
        }
    }
}
