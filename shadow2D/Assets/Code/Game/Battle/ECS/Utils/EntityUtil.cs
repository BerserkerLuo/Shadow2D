
using UnityEngine;
using Table;

namespace ECS
{
    partial class LogicUtils
    {
        public static float GetTime(Entity le) { return Time.time; }//return le.EcsWorld.Time; }
        public static Entity GetEntity(ECSWorld logicWorld, int eid) { return logicWorld.GetBaseEntity(eid); }

        public static Vector2 GetPos(Entity le) {
            AvatarComponent avComp = le.GetComponentData<AvatarComponent>();
            if(avComp == null)
                return Vector2.zero;
            return avComp.Position;
        }

        public static void SetPos(Entity le, Vector3 pos){
            AvatarComponent avComp = le.GetComponentData<AvatarComponent>();
            if (avComp == null)
                return;
            avComp.Position = pos;
        }

        public static Vector3 GetBulletPos(Entity e){
            ECSModelObject obj = AvatarDataUtil.GetEntityMainObj(e);
            if (obj == null || obj.BulletPos == null)
                return GetPos(e);
            return obj.BulletPos;
        }

        public static Vector3 GetForward(Entity e) {
            AvatarComponent avComp = e.GetComponentData<AvatarComponent>();
            if (avComp == null)
                return Vector3.zero;
            return avComp.Forward;
        }

        public static void SetForward(Entity le, Vector3 dire){
            AvatarComponent avComp = le.GetComponentData<AvatarComponent>();
            if (avComp == null)
                return;

            float zAngle = Mathf.Atan2(dire.y, dire.x) * (180 / Mathf.PI);
            avComp.eulerAngles = new Vector3(0, 0, zAngle);
        }

        public static void LookAtTarget(Entity e, Entity target){
            Vector2 dire = Vector2.down;
            if (target != null)
                dire = GetPos(target) - GetPos(e);
            AnimationUtil.SetTargetDire(e,dire);
            AnimationUtil.SetWeaponTarget(e, target);
        }

        public static Entity GetMaster(Entity e) {
            MasterComponent comp = e.GetComponentData<MasterComponent>();
            if (comp == null || comp.Master == null)
                return e;
            return comp.Master; 
        }

        //获得一个entity的类型
        public static int GetEntityType(Entity e){
            var bc = e.GetComponentData<BasicComponent>();
            if (null == bc)
                return EnumEntityType.eNone;

            return bc.Type;
        }

        public static int GetXmlId(Entity e) {
            var bc = e.GetComponentData<BasicComponent>();
            if (null == bc)
                return -1;
            return bc.XmlId;
        }

        //是否死亡
        public static bool IsDead(ECSWorld world,int eId)
        {
            Entity e = GetEntity(world,eId);
            if (e == null)
                return true;
            return IsDead(e);
        }

        //是否死亡
        public static bool IsDead(Entity e)
        {
            return AttrUtil.GetHP(e) <= 0;
        }

        //杀死entity
        public static void KillEntity(Entity e)
        {
            if (e.EcsWorld == null)
                return;

            int type = GetEntityType(e);
            switch (type) {
                case EnumEntityType.eMonster:
                    OnMonsterDeadDrop(e);
                    TryDelayKill(e, 2);
                    break;
                default:
                    TryDelayKill(e,0);
                    break;
            }

            RVOUtil.OnEntityDead(e);

            DebugMgr.OnEndtiyBeKill(e);
        }

        public static void TryDelayKill(Entity e,float delayTime = 3) {
            DelayDeadComponent delayDeadComponent = e.EcsWorld.GlobalEntity.GetComponentData<DelayDeadComponent>();
            if (delayDeadComponent == null) {
                e.EcsWorld.KillEntity(e);
                return;
            }

            if (delayDeadComponent.delayDeadTimes.ContainsKey(e.Eid))
                return;

            delayDeadComponent.delayDeadTimes.Add(e.Eid, GetTime(e) + delayTime);

            AnimationUtil.OnDead(e);
        }

        public static void OnMonsterDeadDrop(Entity e) {
            int xmlId = GetXmlId(e); 

            Debug.Log($"OnMonsterDeadDrop xmlId {xmlId}");

            MonsterCfg monsterCfg = TableMgr.Singleton.GetMonsterCfg(xmlId);
            if (monsterCfg == null || monsterCfg.DropID == 0)
                return;

            Debug.Log($"OnMonsterDeadDrop1 {monsterCfg.DropID }");

            var result = DropUtils.ExecuteDrop(monsterCfg.DropID);
            ItemBattleCfg itemCfg = TableMgr.Singleton.GetItemBattleCfg(result.Key);
            if (itemCfg == null)
                return;

            Debug.Log($"OnMonsterDeadDrop2 {result.Key}");

            CreateUtil.CreateItem(e.EcsWorld,result.Key, LogicUtils.GetPos(e));
        }
    }
}
