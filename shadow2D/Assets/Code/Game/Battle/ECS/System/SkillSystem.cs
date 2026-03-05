
namespace ECS
{
    class SkillSystem : System
    {
        public SkillSystem(ECSWorld world)
        {
            Init(world);
            RequireComponent(typeof(SkillComponent));
        }

        public override void Update(){
            foreach (var e in entities){
                var skillcom = e.GetComponentData<SkillComponent>();
                if (skillcom == null){
                    continue;
                }

                SkillUtils.StepUpdateSkillUnit(skillcom);
            }
        }
    }
}
