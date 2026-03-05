
using Pathfinding.RVO;
using System;
using UnityEngine;

namespace ECS
{
    public class RVOSystem : System
    {
        public RVOSystem(ECSWorld world)
        {
            Init(world);

            RequireComponent(typeof(RVOComponent));
        }

        public override void Update()
        {
            foreach (var e in entities)
            {
                UpdatePos(e);
            }
        }

        public void UpdatePos(Entity e)
        {
            RVOComponent comp = e.GetComponentData<RVOComponent>();
            if (comp == null)
                return;

            Vector2 moveDelta = RVOMgr.Singleton.GetMoveDelta(comp.AgentNo);
            Vector2 newPos = LogicUtils.GetPos(e) + moveDelta;

            LogicUtils.SetPos(e, newPos);
            RVOMgr.Singleton.SetAgentPosition(comp.AgentNo, newPos);

            //float dist = moveDelta.sqrMagnitude * 1000000;
            //if (dist < 0.001f)
            //{
            //    //AnimationUtil.SetMoveDire(e, 0, -1);
            //    AnimationUtil.StopMove(e);
            //}
            //else
            //{
            //    //AnimationUtil.SetMoveDire(e, moveDelta.x, moveDelta.y);
            //    AnimationUtil.Walk(e, 3);
            //}
        }
    }
}