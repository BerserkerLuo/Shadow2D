
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    partial class AutoMoveUpdate
    {
        public static void Update_MoveByPath(Entity e, AutoMoveParamBase moveParam)
        {
            OnPathMove(e, (PathMoveParam)moveParam);
        }

        public static bool OnPathMove(Entity e, PathMoveParam moveParam) {
            return OnPathMove(e, moveParam, moveParam.PathList);
        }

        public static bool OnPathMove(Entity e, AutoMoveParamBase moveParam, List<Vector2> pathList) {
            if (moveParam.IsArriveDestination)
                return true;

            if (pathList == null || pathList.Count == 0){
                moveParam.IsArriveDestination = true;
                return true;
            }

            var speed = AutoMoveToolUtils.GetMoveSpeed(e,moveParam);
            Vector2 srcPos = LogicUtils.GetPos(e);
            float moveLenth = speed * Time.deltaTime;

            var result = GetPathMoveNewPos(pathList, srcPos, moveLenth);

            Vector2 nextPos = Vector2.zero;
            if (pathList.Count <= result.Item2)
                nextPos = pathList[pathList.Count - 1];
            else
                nextPos = pathList[result.Item2];

            Vector2 dire = (nextPos - srcPos).normalized;

            if(!moveParam.IsForceMove)
                LogicUtils.SetForward(e, dire);

            LogicUtils.SetPos(e, result.Item1);

            pathList.RemoveRange(0, result.Item2);

            moveParam.IsArriveDestination = pathList.Count < 1;
            return moveParam.IsArriveDestination;
        }


        //计算单位在一段路径上最新的行进点
        public static (Vector2, int) GetPathMoveNewPos(List<Vector2> pathList, Vector2 curPos, float moveLength, int index = 0, int loop = 100)
        {
            //安全性检查
            if (loop > 100) loop = 100;
            if (loop <= 0 || moveLength <= 0.001f || pathList.Count <= index)
                return (curPos, 0);

            Vector2 targetPos = pathList[index];
            float distence = (curPos - targetPos).magnitude;
            if (moveLength < distence)
            {
                //如果剩余行进距离不足以走完到下一个点
                Vector2 dire = targetPos - curPos;
                Vector2 newPos = curPos + dire.normalized * moveLength;
                return (newPos, 0);
            }

            //剩余进行距离可以走到下一个点 就往下下一个点走
            var result = GetPathMoveNewPos(pathList, targetPos, moveLength - distence, index + 1, loop - 1);
            result.Item2 += 1;
            return result;
        }
    }
}
