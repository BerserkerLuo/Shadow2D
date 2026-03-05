
using System.Collections.Generic;

namespace ECS
{
    internal class SearchTargetUtils
    {
        //===============================================================================================================
        public static List<Entity> SearchEnemyByRange(Entity e,float range,int num = -1) {
            int factionId = FactionUtil.GetFaction(e);
            List<Entity> eList = LogicUtils.GetEntitysByExcludeFactionId(e, factionId);
            List<SearchInfo> ret = FilterUtils.filterRange(LogicUtils.GetPos(e), range, eList, e.Eid);
            return FilterUtils.ConvertEntityList(ret,num);
        }

        public static List<Entity> SearchFriendByRange(Entity e, float range, int num = -1){
            int factionId = FactionUtil.GetFaction(e);
            List<Entity> eList = LogicUtils.GetEntitysByFactionIdList(e, factionId);
            List<SearchInfo> ret = FilterUtils.filterRange(LogicUtils.GetPos(e), range, eList, e.Eid);
            return FilterUtils.ConvertEntityList(ret, num);
        }

        //===============================================================================================================

    }
}
