
namespace ECS
{
    internal class CheckHaveTarget : ATNodeLeaf
    {
        SkillTargetType targetType;
        public CheckHaveTarget(SkillTargetType type) {
            targetType = type;
        }
        public override bool Run(Entity e)
        {
            Entity target = AIDataUtils.GetTarget(e);
            if (target == null)
                return false;

            SkillTargetType type = FactionUtil.GetTargetType(e, target);

            DebugUtils.OnAIThink("CheckHaveTarget2 type {} targetType {}", type, targetType);

            return type == targetType;
        }
    }
}
