


using Table;
using UnityEngine;

namespace ECS
{
    internal class CheckArriveDestination : ATNodeLeaf
    {
        public override bool Run(Entity e){
            DebugUtils.OnAIThink("CheckArriveDestination ");
            if (!LogicDataUtils.IsDoingMove(e))
                return true;
            return LogicDataUtils.IsArriveDestination(e);
        }
    }
}
