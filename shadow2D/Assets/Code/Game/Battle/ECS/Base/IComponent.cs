
namespace ECS
{
    public interface IComponent
    {
        public bool IsAlive { get; set; }

        void Init(Entity e);

        void UnInit();
    }

    public abstract class Component : IComponent
    {
        protected Entity m_entity;

        public bool IsAlive { get; set; }

        public virtual void Init(Entity e){m_entity = e;}

        public Entity Entity { get { return m_entity; } }

        public virtual void AfterInit(){}
        public virtual void UnInit() { }
    }
}
