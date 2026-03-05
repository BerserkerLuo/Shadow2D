
namespace ECS
{
    public class ModBase
    {
        static public string GetName() { return "ModBase"; }

        public ECSWorld world = null;

        public virtual void Init(ECSWorld lw) { world = lw; }
        public virtual void Clear() { world = null; }

        public virtual void OnUpDate() { }
    }
}
