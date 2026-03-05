
using UnityEngine;

namespace ECS
{
    partial class AutoMoveUpdate
    {
        public static void Update_MoveToDirection(Entity e, AutoMoveParamBase moveParam){
            MoveToDirection(e, (MoveToDirectParam)moveParam);
        }

        public static void MoveToDirection(Entity e, MoveToDirectParam moveParam){
            var speed = AutoMoveToolUtils.GetMoveSpeed(e, moveParam);
            Vector3 srcPos = LogicUtils.GetPos(e);
            LogicUtils.SetPos(e,srcPos + moveParam.MoveDir * speed * Time.deltaTime);
        }
    }
}
