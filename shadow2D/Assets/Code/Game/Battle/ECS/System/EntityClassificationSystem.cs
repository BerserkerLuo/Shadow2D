
namespace ECS
{
    class EntityClassificationSystem : System
    {
        public EntityClassificationSystem(ECSWorld ecsWorld)
        {
            Init(ecsWorld);

            RequireComponent(typeof(BasicComponent));
        }

        public override void AddEntity(Entity e)
        {
            base.AddEntity(e);
            LogicUtils.OnAddEntity(EcsWorld,e);
        }

        public override void RemoveEntity(Entity e)
        {
            base.RemoveEntity(e);
            LogicUtils.OnRemoveEntity(EcsWorld, e);
        }

    }

}