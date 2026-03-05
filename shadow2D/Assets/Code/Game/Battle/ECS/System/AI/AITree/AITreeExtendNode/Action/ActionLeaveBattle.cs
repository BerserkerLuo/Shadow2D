using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ECS
{
    internal class ActionLeaveBattle : ATNodeLeaf
    {
        public override bool Run(Entity e){
            AIDataUtils.SetTarget(e, null);
            LogicUtils.LookAtTarget(e, null);
            AnimationUtil.SetMoveDire(e, Vector2.down);
            AnimationUtil.StopMove(e);
            AutoMoveToolUtils.StopMove(e);
            return true;
        }
    }
}
