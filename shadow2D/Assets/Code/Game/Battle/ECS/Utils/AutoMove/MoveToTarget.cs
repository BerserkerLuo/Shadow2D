

using UnityEngine;

namespace ECS
{

    partial class AutoMoveUpdate
    {
        public static void Update_MoveToTarget(Entity e, AutoMoveParamBase moveParam)
        {
            MoveToTarget(e, (MoveToTargetParam)moveParam);
        }

        public static void MoveToTarget(Entity e, MoveToTargetParam moveParam)
        {
            if (moveParam.TargetEntity != null && !LogicUtils.IsDead(moveParam.TargetEntity)){
                var tEPos = LogicUtils.GetPos(moveParam.TargetEntity);
                if (tEPos != Vector2.zero){
                    var srcPos = LogicUtils.GetPos(e);
                    Vector3 step = tEPos - srcPos;
                    if (step.sqrMagnitude > moveParam.KeepDistance * moveParam.KeepDistance)
                        moveParam.TargetPos = tEPos - moveParam.KeepDistance * (tEPos - srcPos).normalized;
                    else
                        moveParam.TargetPos = srcPos;
                }
            }

            MoveToPos(e, moveParam, moveParam.TargetPos);
            //if (moveParam.IsArriveDestination)
            //    AutoMoveToolUtils.StopRVOMove(e);
        }
    }




}
