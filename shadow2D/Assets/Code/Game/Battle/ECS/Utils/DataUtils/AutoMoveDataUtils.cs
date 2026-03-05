
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    partial class LogicDataUtils
    {

        //这个未来要做成一个优先级
        public static bool CheckCanSetAutoMove(Entity e)
        {
            //if (LogicUtils.IsDashState(e))
            //    return false;

            //if (LogicUtils.IsJumpState(e))
            //    return false;

            //if (LogicUtils.IsAbsorbState(e))
            //    return false;

            return true;
        }

        public static void SetSideMove( AutoMoveParamBase moveParam, AutoMoveComponent comp)
        {
            comp.SideAutoMove = moveParam;
        }


        public static void SetMoveParam(bool isTempMove, AutoMoveParamBase moveParam, AutoMoveComponent comp) {
            if (!isTempMove) 
                comp.BaseMoveParam = moveParam;
            else 
                comp.TempMoveParam = moveParam;
        }

        public static AutoMoveParamBase GetMoveParam(AutoMoveComponent autoMoveData)
        {
            if (autoMoveData.TempMoveParam != null && autoMoveData.TempMoveParam.IsStop == false)
                return autoMoveData.TempMoveParam;
            return autoMoveData.BaseMoveParam;
        }

        public static AutoMoveParamBase GetMoveParam(Entity e) {
            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return null;
            return GetMoveParam(autoMoveData);
        }

        public static bool IsArriveDestination(Entity e){
            AutoMoveParamBase param = GetMoveParam(e);
            if (param == null)
                return false;
            return param.IsArriveDestination;
        }

        public static bool IsDoingMove(Entity e) {
            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return false;
            if (autoMoveData.BaseMoveParam != null && !autoMoveData.BaseMoveParam.IsStop)
                return true;
            if (autoMoveData.TempMoveParam != null && !autoMoveData.TempMoveParam.IsStop)
                return true;
            return false;
        }

        //是否正在执行某种移动操作
        public static bool IsDoingMove(AutoMoveComponent autoMoveData, int moveType){
            var moveParam = GetMoveParam(autoMoveData);
            if (moveParam == null)
                return false;
            if (moveParam.MoveType != moveType)
                return false;
            return true;
        }

        //跟随目标单位移动
        public static void SetTargetMove(Entity e,Entity taget,float keepDistance  = 0,bool isTempMove = false) {
            if (!CheckCanSetAutoMove(e))
                return;

            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return;

            MoveToTargetParam moveParam = new MoveToTargetParam();

            moveParam.TargetEntity = taget;
            moveParam.KeepDistance = keepDistance;

            SetMoveParam(isTempMove,moveParam, autoMoveData);
        }
        
        //向目标点移动
        public static void SetPosMove(Entity e,Vector3 tarPos,bool isTempMove =false) {
            if (!CheckCanSetAutoMove(e))
                return;

            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return;

            MoveToPosParam moveParam = new MoveToPosParam();

            moveParam.TargetPos = tarPos;

            SetMoveParam(isTempMove, moveParam, autoMoveData);
        }

        //路径移动
        public static void SetPathMove(Entity e, List<Vector2> path, bool isTempMove = false)
        {
            if (!CheckCanSetAutoMove(e))
                return;

            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return;

            PathMoveParam moveParam = new PathMoveParam();

            moveParam.PathList = path;

            SetMoveParam(isTempMove, moveParam, autoMoveData);
        }

        //环绕移动
        public static void SetSurroundMove(Entity e,Entity target, float AngularVelocity, float EccentricDistance,Vector3 StartSurroundAngle, bool isTempMove = false) {
            if (!CheckCanSetAutoMove(e))
                return;

            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return;

            if (IsDoingMove(autoMoveData, AutoMoveType.AutoMove_SurroundMove))
                return;

            SurroundMoveParam moveParam = new SurroundMoveParam();

            moveParam.TargetEntity = target;
            moveParam.TargetPos = LogicUtils.GetPos(target);
            moveParam.AngularVelocity = AngularVelocity;
            moveParam.EccentricDistance = EccentricDistance;
            moveParam.SurroundAngle = StartSurroundAngle;

            SetMoveParam(isTempMove, moveParam, autoMoveData);
        }


        public static void SetFindPathToPosMove(Entity e, Vector3 pos, float keepDistance = 0, bool isTempMove = false)
        {
            if (!CheckCanSetAutoMove(e))
                return;

            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return;

            //DebugUtils.Log("SetFindPathToPosMove eid {}", e.Eid);

            FindPathPosMoveParam moveParam = new FindPathPosMoveParam();

            moveParam.pos = pos;
            moveParam.keepDistance = keepDistance;

            SetMoveParam(isTempMove, moveParam, autoMoveData);
        }

        //物品拾取
        public static void SetPickUpMove(Entity e,Entity target, bool isTempMove = false)
        {
            if (!CheckCanSetAutoMove(e))
                return;

            AutoMoveComponent autoMoveData = e.GetComponentData<AutoMoveComponent>();
            if (autoMoveData == null)
                return;

            if (IsDoingMove(autoMoveData, AutoMoveType.AutoMove_PickUpMove))
                return;

            PickUpMoveParam moveParam = new PickUpMoveParam();

            Vector3 pos1 = LogicUtils.GetPos(e);
            Vector3 pos2 = LogicUtils.GetPos(target);
            Vector3 dire = pos1 - pos2;

            moveParam.UseForceMoveSpeed = true;
            moveParam.ForceMoveSpeed = 10;
            moveParam.TargetEntity = target;
            moveParam.TargetPos = pos1 + dire.normalized * 2;
            moveParam.IsFaceToTarget = false;

            SetMoveParam(isTempMove, moveParam, autoMoveData);
        }

        public static void SetPauseMove(Entity e, bool isPause){
            AutoMoveComponent comp = e.GetComponentData<AutoMoveComponent>();
            if (comp == null || comp.BaseMoveParam == null)
                return;

            comp.BaseMoveParam.IsPauseMove = isPause;
        }
    }
}
