
using System.Collections.Generic;

namespace ECS
{
    internal class LockerComponent : Component
    {
        public Dictionary<string, object> Objs = new Dictionary<string, object>();
    }
}
