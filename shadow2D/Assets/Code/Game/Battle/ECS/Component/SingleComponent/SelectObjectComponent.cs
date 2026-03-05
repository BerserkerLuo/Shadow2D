
using System.Collections.Generic;

namespace ECS
{
    //单例Component
    public class SelectObjectComponent : Component
    {
        public Entity ShowEntity = null;
        public List<Entity> SelectList = new List<Entity>();

    }
}
