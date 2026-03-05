using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS
{
    internal class ActionStopMove : ATNodeLeaf
    {
        public override bool Run(Entity e){
            AutoMoveToolUtils.StopMove(e);
            return true;
        }
    }
}
