
using System;
using Unity.Mathematics;
using UnityEngine;

namespace ECS
{
    partial class AutoMoveUpdate
    {
        public static void Update_MoveToPos(Entity e, AutoMoveParamBase moveParam)
        {
            MoveToPos(e, (MoveToPosParam)moveParam);
        }

        public static void MoveToPos(Entity e, MoveToPosParam moveParam)
        {
            if (MoveToPos(e, moveParam, moveParam.TargetPos))
                AutoMoveToolUtils.StopMove(e);
        }

        //这里提供一个干净的纯粹的移动 会有多个功能使用到 
        public static bool MoveToPos(Entity e, AutoMoveParamBase moveParam, Vector2 targetPos) {

            //DebugMgr.UpdateTargetPos(e,targetPos);

            RVOComponent comp = e.GetComponentData<RVOComponent>();
            if (comp != null && comp.AgentNo != -1)
                return MoveToPosByRVO(e, moveParam, comp, targetPos);
            else
                return MoveToPos_(e,moveParam,targetPos);
        }
        public static bool MoveToPos_(Entity e, AutoMoveParamBase moveParam,Vector2 targetPos)
        {
            var srcPos = LogicUtils.GetPos(e);
            // 速度
            float speed = AutoMoveToolUtils.GetMoveSpeed(e,moveParam);

            var step = speed * Time.deltaTime;
            var distance = (targetPos - srcPos).magnitude;
            if (distance > step) 
            {
                Vector2 dire = (targetPos - srcPos).normalized;
                var pos = srcPos + dire * step;
                LogicUtils.SetPos(e, pos);

                if (moveParam.IsFaceToTarget)
                    LogicUtils.SetForward(e,dire);

                return false;
            }
            else
            {
                //这一步到位了
                LogicUtils.SetPos(e, targetPos);
                moveParam.IsArriveDestination = true;

                return true;
            }
        }

        public static bool MoveToPosByRVO(Entity e, AutoMoveParamBase moveParam, RVOComponent comp,Vector2 targetPos) {

            Vector2 entityPos = LogicUtils.GetPos(e);
            if (LogicUtils.GetSqrDistance(entityPos, targetPos) < 0.001f){
                moveParam.IsArriveDestination = true;
                RVOUtil.SetAgentStop(e);
                return true;
            }

            RVOUtil.SetAgentTarget(e, targetPos);
            return false;
        }


    }



}
