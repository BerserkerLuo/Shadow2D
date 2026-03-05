


using Table;
using UnityEngine;

namespace ECS
{
    internal class CheckIsInState : ATNodeLeaf
    {
        public AIState state;
        public CheckIsInState(AIState state) {
            this.state = state;
        }
        public override bool Run(Entity e)
        {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return false;

            DebugUtils.OnAIThink("CheckIsInState state {}", state);

            return comp.AIState == state;
        }

    
    }
}
