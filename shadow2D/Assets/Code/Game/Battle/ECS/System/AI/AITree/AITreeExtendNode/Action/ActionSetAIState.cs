


using Table;
using UnityEngine;


namespace ECS
{
    internal class ActionSetAIState : ATNodeLeaf
    {
        public AIState state;
        public ActionSetAIState(AIState state) {
            this.state = state;
        }
        public override bool Run(Entity e)
        {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return false;

            DebugUtils.OnAIThink("ActionSetAIState {}", state);

            comp.AIState = state;
            comp.AIStateStartTime = Time.time;

            if (state == AIState.AI_Idle)
            {
               
            }

            return true;
        }

    
    }
}
