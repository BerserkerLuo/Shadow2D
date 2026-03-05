
namespace ECS
{
    internal class CheckIsDead : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {
            DebugUtils.OnAIThink("CheckIsDead");

            return LogicUtils.IsDead(e);
        }
    }
}
