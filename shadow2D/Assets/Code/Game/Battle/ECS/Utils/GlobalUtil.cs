
using UnityEngine;

namespace ECS
{
    partial class LogicUtils
    {
        //=================================================================
        public static PlayerComponent GetPlayerComponent(ECSWorld world) {
            return world.GlobalEntity.GetComponentData<PlayerComponent>();
        }

        public static void SetPlayerEntity(ECSWorld world,Entity player) {
            PlayerComponent comp = GetPlayerComponent(world);
            if (comp == null)
                return;

            comp.Player = player;
        }

        public static Entity GetPlayerEntity(ECSWorld world){
            PlayerComponent comp = GetPlayerComponent(world);
            if (comp == null)
                return null;

            return comp.Player;
        }

        //=================================================================
        public static Vector3 GetPlayerPos(ECSWorld world)
        {
            Entity player = GetPlayerEntity(world);
            if (player != null)
                return GetPos(player);

            return Vector3.zero;
        }

        public static Vector3 GetSurroundPlayerPos(ECSWorld world,float max,float min = 0) {
            Vector3 centerPos = Vector3.zero;
            Entity player = GetPlayerEntity(world);
            if (player != null)
                centerPos = GetPos(player);

            return GetSurroundPos(centerPos,max,min);
        }

        public static Vector2 GetSurroundPos(Vector3 centerPos, float max, float min = 0){
            Vector2 v = new Vector2(0, GetRand(min, max));
            return Quaternion.Euler(0, 0, GetRand(0.0f,360.0f)) * v;
        }

        //=================================================================
        public static GlobalComponent GetGlobalComponent(ECSWorld world){
            return world.GlobalEntity.GetComponentData<GlobalComponent>();
        }

        public static bool GetEndGameFlag(ECSWorld world) {
            GlobalComponent comp = GetGlobalComponent(world);
            if (comp == null)
                return false;
            return comp.IsEnd;
        }

        public static void SetGameEnd(ECSWorld world) {
            GlobalComponent comp = GetGlobalComponent(world);
            if (comp == null) return;
            comp.IsEnd = true;
        }
    }
}
