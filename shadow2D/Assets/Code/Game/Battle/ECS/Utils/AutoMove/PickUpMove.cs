
using UnityEngine;

namespace ECS
{

    partial class AutoMoveUpdate
    {
        public static void Update_PickUpMove(Entity e, AutoMoveParamBase moveParam)
        {
            OnPickUpMove(e,(PickUpMoveParam)moveParam);
        }

        public static void OnPickUpMove(Entity e,PickUpMoveParam moveParam) {
            if (moveParam.TargetPos != Vector3.zero){
                MoveToPos(e, moveParam, moveParam.TargetPos);
                if (!moveParam.IsArriveDestination)
                    return;

                moveParam.TargetPos = Vector3.zero;
                moveParam.ForceMoveSpeed *= 2;
            }

            if (moveParam.TargetEntity == null)
                return;

            Vector2 targetPos = LogicUtils.GetPos(moveParam.TargetEntity);
            MoveToPos(e, moveParam, targetPos);
        }

    }

}
