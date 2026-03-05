
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ECS
{
    partial class AutoMoveUpdate
    {
        public static void Update_AIMove(Entity e, AutoMoveParamBase moveParam)
        {
            OnFindPathPosMove(e, (FindPathPosMoveParam)moveParam);
        }

        public static void OnFindPathPosMove(Entity e, FindPathPosMoveParam moveParam) {

            float dist = LogicUtils.GetSqrDistance(LogicUtils.GetPos(e),moveParam.pos);
            if (dist < moveParam.keepDistance * moveParam.keepDistance || (moveParam.keepDistance == 0 && dist < 0.001f))
            {
                AutoMoveToolUtils.StopMove(e);
                AnimationUtil.SetMoveDire(e, Vector2.down);
                AnimationUtil.StopMove(e);
                moveParam.IsArriveDestination = true;
                return;
            }

            if (moveParam.InSearchPath)
                return;

            if (moveParam.PathList == null || moveParam.PathList.Count == 0)
            {
                moveParam.InSearchPath = true;
                PathFindUtils.FindPathToPos(e, moveParam.pos,(pathList) => {
                    moveParam.PathList = pathList;
                    moveParam.InSearchPath = false;

                    //DebugMgr.DrawPath(e, moveParam.PathList);
                });
                
            }
            if (moveParam.PathList == null || moveParam.PathList.Count == 0)
                return;

            LockerUitls.SetIntValue(e,"moveFlag",1);

            if (moveParam.PathList.Count > 1) {
                Vector3 pos = LogicUtils.GetPos(e);
                Vector2 direVec = moveParam.PathList[0] - pos;
                if (direVec.sqrMagnitude < 0.35f)
                    moveParam.PathList.RemoveAt(0);
                else
                {
                    AnimationUtil.SetMoveDire(e, direVec.normalized);
                    AnimationUtil.Walk(e, AttrUtil.GetSpeed(e));
                }
            }
 
            Vector3 pathPos = moveParam.PathList[0];
            if (MoveToPos(e, moveParam, pathPos)) 
                moveParam.PathList.RemoveAt(0);

            //if (moveParam.PathList.Count == 0){
            //    AutoMoveToolUtils.StopMove(e);
            //}
        }

    }



}
