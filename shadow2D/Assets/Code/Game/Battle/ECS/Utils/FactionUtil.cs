
namespace ECS
{
    internal class FactionUtil
    {
        public static void SetFaction(Entity e,int factionId) {
            FactionComponent facComp = e.GetComponentData<FactionComponent>();
            if (facComp == null)
                return;
            facComp.FactionId = factionId;
        }

        public static int GetFaction(Entity e){
            Entity master = LogicUtils.GetMaster(e);
            FactionComponent facComp = master.GetComponentData<FactionComponent>();
            if (facComp == null)
                return -1;
            return facComp.FactionId;
        }

        public static bool IsEnemy(Entity left,Entity right) {
            return GetFaction(left) != GetFaction(right);
        }

        public static SkillTargetType GetTargetType(Entity left, Entity right) {
            return IsEnemy(left, right) ? SkillTargetType.Enemy : SkillTargetType.Frend;
        }
    }
}
