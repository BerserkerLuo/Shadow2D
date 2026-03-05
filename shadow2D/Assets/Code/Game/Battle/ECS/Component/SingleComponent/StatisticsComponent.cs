
using System.Collections.Generic;
using Table;
using UnityEngine;

namespace ECS
{

    //统计组件
    public class StatisticsComponent : Component{
        public Dictionary<string,int> StatisticsValues = new();
    }
}
