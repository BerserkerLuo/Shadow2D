

using Game;
using UnityEngine;

namespace ECS
{
    public class CameraFollowSystem : System
    {
        public CameraFollowSystem(ECSWorld world) {
            Init(world);
        }

        public override void Update(){

            Entity player = LogicUtils.GetPlayerEntity(EcsWorld);
            if (player == null)
                return;

            Transform CameraRoot = CameraMgr.CameraRoot;
            if (CameraRoot == null)
                return;

            Vector3 cPos = CameraRoot.position;
            Vector3 pos = LogicUtils.GetPos(player);
            if (cPos == pos)
                return;

            CameraRoot.position = Vector3.Lerp(cPos, pos, 0.3f);
        }

    }
}
