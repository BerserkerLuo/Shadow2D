
namespace ECS
{
    internal class ActionUseSkillToTarget : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {
            DebugUtils.OnAIThink("ActionAutoUseSkill Start EID {}", e.Eid);

            Entity target = AIDataUtils.GetTarget(e);
            if (target == null)
                return false;

            int skillId = AIDataUtils.GetUseSkillId(e);
            if (skillId == -1) {
                int targetType = FactionUtil.IsEnemy(target, e) ? 0 : 1;
                skillId = SkillDataUtil.GetFreeSkill(e, targetType);
            }
            if (skillId == -1)
                return false;

            LogicUtils.LookAtTarget(e,target);

            DebugUtils.OnAIThink("ActionAutoUseSkill EID {} {}", e.Eid,skillId);

            SkillUtils.CastSKillToTargetBySkillId(e, skillId, target.Eid);

            return true;
        }
    }
}
