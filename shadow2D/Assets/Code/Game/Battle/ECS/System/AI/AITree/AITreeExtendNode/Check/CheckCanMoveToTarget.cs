
using UnityEngine;

namespace ECS
{
    internal class CheckCanMoveToTarget : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {
            DebugUtils.OnAIThink("CheckCanMoveToTarget1 ");

            AIComponent comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return false;

            if (comp.Target == null || LogicUtils.IsDead(comp.Target))
                return false;

            float dist =LogicUtils.GetSqrDistance(comp.FollowPos,LogicUtils.GetPos(comp.Target));
            if (dist < comp.SqrKeepDistance)
                return false;

            DebugUtils.OnAIThink("CheckCanMoveToTarget2 ");
            return true;
        }
    }
}
