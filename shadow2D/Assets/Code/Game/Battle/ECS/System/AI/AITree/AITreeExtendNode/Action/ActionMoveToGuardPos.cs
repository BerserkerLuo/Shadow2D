using UnityEngine;
namespace ECS
{
    internal class ActionMoveToGuardPos : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {

            Vector3 guardPos = AIDataUtils.GetGuardPos(e);
            if (guardPos == Vector3.zero)
                return true;

            LogicDataUtils.SetFindPathToPosMove(e, guardPos);

            DebugUtils.OnAIThink("ActionMoveToGuardPos");

            return true;
        }
    }
}
