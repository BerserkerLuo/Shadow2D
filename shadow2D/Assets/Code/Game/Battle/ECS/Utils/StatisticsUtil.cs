
using System.Collections.Generic;

namespace ECS
{
    internal class StatisticsUtil
    {

        static public void AddStatisticsCount(StatisticsComponent comp,string key,int v) {
            int oldV = comp.StatisticsValues.GetValueOrDefault(key,0);
            comp.StatisticsValues.Add(key,oldV + v);
        }
        static public int GetStatisticsCount(StatisticsComponent comp,string key){
            return comp.StatisticsValues.GetValueOrDefault(key,0);
        }

        static public void AddStatisticsCount(ECSWorld world, string key, int v) {
            StatisticsComponent comp = world.GlobalEntity.GetComponentData<StatisticsComponent>();
            AddStatisticsCount(comp,key,v);
        }
        static public int GetStatisticsCount(ECSWorld world, string key){
            StatisticsComponent comp = world.GlobalEntity.GetComponentData<StatisticsComponent>();
            return GetStatisticsCount(comp, key);
        }

        //===========================================================================

        static public void OnSkillMonster(Entity atker,Entity tar) {
            Entity master = LogicUtils.GetMaster(atker);
            if (LogicUtils.GetEntityType(master) != EnumEntityType.eHero &&
                LogicUtils.GetEntityType(tar) != EnumEntityType.eMonster)
                return;
            AddStatisticsCount(master.EcsWorld,"KillCount",1);
        }

        static public int GetSkillMonsterCount(ECSWorld world) {
            return GetStatisticsCount(world,"KillCount");
        }
    }
}
