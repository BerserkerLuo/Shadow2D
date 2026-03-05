
using System.Collections.Generic;

namespace ECS
{
    public class EntityClassificationComponent : Component
    {
        //map<FactionId,map<Eid,Entity>>
        public Dictionary<int,Dictionary<int,Entity> > DictEntityClass = new Dictionary<int, Dictionary<int, Entity>>();
    }
}