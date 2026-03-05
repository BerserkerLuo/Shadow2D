using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public delegate void OnFindPathCallBack(List<Vector3> path);

    public class PathFindUtils
    {
        static public bool MergePath(List<Vector3> path, List<Vector3> newPath)
        {
            if (path.Count < 3)
            {
                newPath.AddRange(path);
                return false;
            }

            var prevPos = path[0];
            var midPos = path[1];

            Vector3 dire1 = (midPos - prevPos).normalized;

            //newPath.Add(prevPos);
            for (var i = 2; i < path.Count; ++i)
            {
                var nextPos = path[i];
                Vector3 dire2 = (nextPos - midPos).normalized;
                if (dire2 != dire1)
                {
                    dire1 = dire2;
                    newPath.Add(prevPos);
                    prevPos = midPos;
                }
                midPos = nextPos;
            }

            newPath.Add(prevPos);
            newPath.Add(midPos);

            return true;
        }

        public static void FindPathToPos(Entity e,Vector3 target, OnFindPathCallBack callback) {
            ECSModelObject modelObj = AvatarDataUtil.GetEntityMainObj(e);
            if (modelObj == null) 
                return;
            Seeker seeker = modelObj.Seeker;
            if (seeker == null)
                return;
            seeker.StartPath(LogicUtils.GetPos(e),target, (path ) => {
                List<Vector3> retPath = new List<Vector3>();
                MergePath(path.vectorPath, retPath);
                callback(retPath);
            });
        }
    }
}