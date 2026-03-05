
using System.Collections.Generic;

namespace ECS
{
    public delegate void OnAutoMoveFunction(Entity entity, AutoMoveParamBase moveParam);

    public class AutoMoveDispatcher
    {
        private static Dictionary<int, OnAutoMoveFunction> g_dictOnAutoMoveFunctions = new Dictionary<int, OnAutoMoveFunction>();

        public static void RegisterAllBaseMoveFunction() {
            RegisterMoveFunction(AutoMoveType.AutoMove_Direction, AutoMoveUpdate.Update_MoveToDirection);
            RegisterMoveFunction(AutoMoveType.AutoMove_Pos, AutoMoveUpdate.Update_MoveToPos);
            RegisterMoveFunction(AutoMoveType.AutoMove_ToTarget, AutoMoveUpdate.Update_MoveToTarget);
            RegisterMoveFunction(AutoMoveType.AutoMove_Path, AutoMoveUpdate.Update_MoveByPath);
            RegisterMoveFunction(AutoMoveType.AutoMove_SurroundMove, AutoMoveUpdate.Update_SurroundMove);
            RegisterMoveFunction(AutoMoveType.AutoMove_FindPathToPos, AutoMoveUpdate.Update_AIMove);
            RegisterMoveFunction(AutoMoveType.AutoMove_PickUpMove, AutoMoveUpdate.Update_PickUpMove);
            
        }

        public static void ClearTactics()
        {
            g_dictOnAutoMoveFunctions.Clear();
        }

        public static void RegisterMoveFunction(int moveType, OnAutoMoveFunction moveFun)
        {
            if (g_dictOnAutoMoveFunctions.ContainsKey(moveType) == false)
                g_dictOnAutoMoveFunctions.Add(moveType, moveFun);
            else
                //允许Mod重写基础移动
                g_dictOnAutoMoveFunctions[moveType] = moveFun;
        }

        public static OnAutoMoveFunction GetMoveFunction(int moveType)
        {
            OnAutoMoveFunction moveFun;
            if (g_dictOnAutoMoveFunctions.TryGetValue(moveType, out moveFun) == false)
                return null;
            return moveFun;
        }

    }
}
