
using System.Collections.Generic;

namespace ECS
{
    //延迟死亡
    public class DelayDeadComponent : Component
    {
        //Eid => 删除时间
        public Dictionary<int, float> delayDeadTimes = new Dictionary<int, float>();
    }
}
