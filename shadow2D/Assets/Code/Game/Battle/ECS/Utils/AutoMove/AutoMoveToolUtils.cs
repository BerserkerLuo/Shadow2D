

namespace ECS
{
    public class AutoMoveToolUtils
    {
        public static float GetMoveSpeed(Entity e, AutoMoveParamBase moveParam) {
            if (moveParam.UseForceMoveSpeed == false)
                return AttrUtil.GetSpeed(e);
            return moveParam.ForceMoveSpeed;
        }

        //停止自动移动 
        public static void StopMove(Entity e)
        {
            RVOUtil.SetAgentStop(e);

            AutoMoveParamBase moveParam = LogicDataUtils.GetMoveParam(e);
            if (moveParam == null)
                return;

            LockerUitls.SetIntValue(e, "moveFlag", 0);

            moveParam.IsStop = true;
        }
    }
}
