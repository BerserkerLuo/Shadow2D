


using Table;
using UnityEngine;

namespace ECS
{
    //检查 AI状态的保持时间是否 > 设定值
    internal class CheckStateTime : ATNodeLeaf
    {
        public AIState state;
        public float time;

        public CheckStateTime(AIState state,float time,int checkFlag = 0) {
            this.state = state;
            this.time = time;
        }
        public override bool Run(Entity e)
        {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return false;

            if (comp.AIState != state)
                return false;

            float keepTime = Time.time - comp.AIStateStartTime;

            //Debug.Log("keepTime " + keepTime + " time "+ time);

            return keepTime > time;
        }

    
    }
}
