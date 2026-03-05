
using UnityEngine;

namespace ECS
{
    partial class AutoMoveUpdate
    {
        public static void Update_SurroundMove(Entity e, AutoMoveParamBase moveParam)
        {
            OnSurroundMove(e,(SurroundMoveParam)moveParam);
        }

        public static void OnSurroundMove(Entity e, SurroundMoveParam moveParam) {
            if (!LogicUtils.IsDead(moveParam.TargetEntity))
                moveParam.TargetPos = LogicUtils.GetPos(moveParam.TargetEntity);

            float angle = moveParam.AngularVelocity * Time.deltaTime;
            Quaternion roate = Quaternion.Euler(0, angle, 0);
            moveParam.SurroundAngle = roate * moveParam.SurroundAngle;
            Vector3 movePos = moveParam.TargetPos + moveParam.SurroundAngle.normalized * moveParam.EccentricDistance;
            LogicUtils.SetPos(e, movePos);
            LogicUtils.SetForward(e, moveParam.SurroundAngle);
        }
    }
}
