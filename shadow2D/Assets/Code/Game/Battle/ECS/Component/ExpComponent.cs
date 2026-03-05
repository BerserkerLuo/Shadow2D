using System.Collections.Generic;

namespace ECS
{
    //经验
    public class ExpComponent : Component
    {
        public int Level = 1;
        public float exp = 0;
        public float lvUpCost = 9; //升级所需经验
    }
}