
using System.Collections.Generic;

namespace ECS
{
    partial class LogicUtils
    {
        //获取单位分类Component [EntityClassificationComponent]
        public static EntityClassificationComponent GetClassificationComponent(Entity le){ return GetClassificationComponent(le.EcsWorld);}
        public static EntityClassificationComponent GetClassificationComponent(ECSWorld logicWorld) {
            if (logicWorld.GlobalEntity == null) return null;
            EntityClassificationComponent comp = logicWorld.GlobalEntity.GetComponentData<EntityClassificationComponent>();
            if (comp == null) return null;
            return comp;
        }

        //获取一个类型集合
        public static Dictionary<int, Entity> GetClassMap(ECSWorld logicWorld, int factionId)
        {
            var comp = GetClassificationComponent(logicWorld);
            if (comp == null)
                return null;

            Dictionary<int, Entity> classMap;
            if (comp.DictEntityClass.TryGetValue(factionId, out classMap) == false)
            {
                //如果不存在就创建一个
                classMap = new Dictionary<int, Entity>();
                comp.DictEntityClass.Add(factionId, classMap);
            }

            return classMap;
        }

        //获取所有阵营ID
        public static List<int> GetAllFactionIdList(ECSWorld logicWorld){
            var comp = GetClassificationComponent(logicWorld);
            if (comp == null)
                return null;
            List<int> retList = new List<int>();
            retList.AddRange(comp.DictEntityClass.Keys);
            return retList;
        }

        //添加单位
        public static void OnAddEntity(ECSWorld logicWorld,Entity addEntity) {
            int factionId = FactionUtil.GetFaction(addEntity);
            Dictionary<int, Entity> classMap = GetClassMap(logicWorld, factionId);
            if(classMap != null)
            classMap.Add(addEntity.Eid,addEntity);
        }

        //删除单位
        public static void OnRemoveEntity(ECSWorld logicWorld, Entity removeEntity)
        {
            int factionId = FactionUtil.GetFaction(removeEntity);
            Dictionary<int, Entity> classMap = GetClassMap(logicWorld, factionId);
            if(classMap != null)
                classMap.Remove(removeEntity.Eid);
        }

        //根据阵营ID获取单位列表
        public static List<Entity> GetEntitysByType(Entity le,int factionId,bool isExcludeDead = true) {
            List<Entity> retList = new List<Entity>();
            GetEntitysByFactionId(le.EcsWorld, retList, factionId, isExcludeDead);
            return retList;
        }
        public static void GetEntitysByFactionId(ECSWorld logicWorld, List<Entity> retList, int factionId, bool isExcludeDead = true) {
            Dictionary<int, Entity> classmap = GetClassMap(logicWorld, factionId);
            if (classmap == null)
                return;
            foreach (var it in classmap)
            {
                Entity e = it.Value;
                if (e == null)
                    continue;

                if (isExcludeDead && IsDead(e))
                    continue;

                retList.Add(it.Value);
            }
        }

        public static List<Entity> GetEntitysByExcludeFactionId(Entity le,int excludeFactionId) {
            List<Entity> retList = new List<Entity>();
            List<int> AllFactionIdList = GetAllFactionIdList(le.EcsWorld);
            if (AllFactionIdList == null)
                return retList;

            foreach (int factionId in AllFactionIdList){
                if (factionId == excludeFactionId)
                    continue;
                GetEntitysByFactionId(le.EcsWorld, retList, factionId);
            }
            return retList;
        }

        //根据类型列表获取单位列表
        public static List<Entity> GetEntitysByFactionIdList(Entity le, List<int> FactionIdList) { return GetEntitysByFactionIdList(le, FactionIdList.ToArray()); }
        public static List<Entity> GetEntitysByFactionIdList(Entity le, params int[] FactionIds) {return GetEntitysByFactionIdList(le.EcsWorld, FactionIds);}
        public static List<Entity> GetEntitysByFactionIdList(ECSWorld lw, params int[] FactionIds)
        {
            List<Entity> retList = new List<Entity>();
            foreach (int type in FactionIds)
                GetEntitysByFactionId(lw, retList, type);
            return retList;
        }
    }
}
