
using UnityEngine;

namespace ECS
{
    internal class AIDataUtils
    {

        public static void SetAITreeType(Entity e, int treeType) {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return;
            comp.treeType = treeType;
        }

        public static void SetTarget(Entity e, Entity target) {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return;
            comp.Target = target;
            comp.SqrKeepDistance = 0f;
            comp.FollowPos = Vector3.zero;
        }
        public static Entity GetTarget(Entity e) {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return null;
            return comp.Target;
        }

        public static void SetUseSkillId(Entity e,int skillId){
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return;
            comp.UseSkillId = skillId;
        }
        public static int GetUseSkillId(Entity e){
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return -1;
            return comp.UseSkillId;
        }

        public static Vector3 GetGuardPos(Entity e) {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return Vector3.zero;
            return comp.GuardPos;
        }

        public static float GetGuardRange(Entity e) {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return 0.0f;
            return comp.GuardRange;
        }
        public static void SetGuardRange(Entity e,float guardRange){
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return;
            comp.GuardRange = guardRange;
        }

        public static void SetAIState(Entity e,AIState aiState) {
            var comp = e.GetComponentData<AIComponent>();
            if (comp == null)
                return;
            comp.AIState = aiState;
        }

    }
}
