using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class BackpackComponent : Component
    {
        public Dictionary<int, int> BackpackDic = new Dictionary<int, int>();
    }
}