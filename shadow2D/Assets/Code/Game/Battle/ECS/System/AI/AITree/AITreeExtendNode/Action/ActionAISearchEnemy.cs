
using System.Collections.Generic;

namespace ECS
{
    internal class ActionAISearchEnemy : ATNodeBase
    {
        public override bool Run(Entity e)
        {
            float range = AIDataUtils.GetGuardRange(e);

            List<Entity> eList = SearchTargetUtils.SearchEnemyByRange(e, range);
            if (eList.Count <= 0)
                return false;

            DebugUtils.OnAIThink("ActionAISearchEnemy {}", eList[0].Eid);

            AIDataUtils.SetTarget(e,eList[0]);
            return true;
        }
    }
}
