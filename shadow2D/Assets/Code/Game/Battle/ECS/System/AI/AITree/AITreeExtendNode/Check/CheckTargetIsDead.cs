
namespace ECS
{
    internal class CheckTargetIsDead : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {
            Entity tar = AIDataUtils.GetTarget(e);
            if (tar == null)
                return true;

            DebugUtils.OnAIThink("CheckTargetIsDead");

            return LogicUtils.IsDead(tar);
        }
    }
}
