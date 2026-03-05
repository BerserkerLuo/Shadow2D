
namespace ECS
{
    class AISystem : System
    {
        public AISystem(ECSWorld world)
        {
            Init(world);
            RequireComponent(typeof(AIComponent));
        }

        public override void Update(){

            float now = EcsWorld.Time;

            foreach (var e in entities){

                var comp = e.GetComponentData<AIComponent>();
                if (comp == null)
                    continue;

                if (now - comp.LastThinkTime < comp.ThinkInterval)
                    return;
                comp.LastThinkTime = now;

                AITree tree = AITreeMgr.GetAITree(comp.treeType);
                if (tree == null)
                    return;

                tree.Tick(e);

                DebugUtils.OnAIThinkEnd(e.Eid);
            }
        }
    }
}
