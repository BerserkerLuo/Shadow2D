


using System.Collections.Generic;

namespace ECS
{
    //Buff组件
    class StatusComponent : Component
    {
        public Dictionary<int, StatusInfo> m_dictStatus = new Dictionary<int, StatusInfo>();
    }
}