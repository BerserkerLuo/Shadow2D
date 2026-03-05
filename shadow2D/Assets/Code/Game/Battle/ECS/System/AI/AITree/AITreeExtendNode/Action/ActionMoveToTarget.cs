
namespace ECS
{
    internal class ActionMoveToFollowTarget : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {
            DebugUtils.OnAIThink("ActionMoveToTarget Start eId {} ", e.Eid);

            Entity target = AIDataUtils.GetTarget(e);
            if (target == null)
                return false; 

            int targetType =  (int)FactionUtil.GetTargetType(target,e);
            float keepDistance = SkillDataUtil.GetFarthestSkillCastDistance(e, targetType) / 2f;

            DebugUtils.OnAIThink("ActionMoveToTarget eId {} keepDistance {}", e.Eid, keepDistance);

            AIComponent aicomp = e.GetComponentData<AIComponent>();
            aicomp.FollowPos = LogicUtils.GetPos(target);
            aicomp.SqrKeepDistance = keepDistance * keepDistance;

            LogicDataUtils.SetFindPathToPosMove(e, aicomp.FollowPos, keepDistance);

            return true;
        }
    }
}
