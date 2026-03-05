 
namespace ECS
{
    internal class CheckTargetIsLeave : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {
            AIComponent comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return true;

            if (comp.Target == null || LogicUtils.IsDead(comp.Target))
                return true;

            float dist = LogicUtils.GetSqrDistance(LogicUtils.GetPos(comp.Target), comp.FollowPos);
            if (dist > comp.SqrKeepDistance)
                return true;

            //AutoMoveParamBase param = LogicDataUtils.GetMoveParam(e);
            //if (param != null && !param.IsArriveDestination)
            //    return true;

            return false;
        }
    }
}
