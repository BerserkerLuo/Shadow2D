
using UnityEngine;

namespace ECS
{
    public class PickupSystem : System
    {
        public PickupSystem(ECSWorld world)
        {
            Init(world);
            RequireComponent(typeof(DropComponent));

            ItemRewardDispatcher.ClearTactics();
            ItemRewardDispatcher.RegisterAllItemRewardFunction();
        }

        public override void Update() {

            Entity player = LogicUtils.GetPlayerEntity(EcsWorld);
            Vector2 pos = LogicUtils.GetPos(player);
            float pickUpRange = AttrUtil.GetAttr(player,AttrType.PickUpRange);
            pickUpRange = pickUpRange * pickUpRange;
            foreach (Entity e in entities) {

                float sqrDist = (LogicUtils.GetPos(e) - pos).SqrMagnitude() ;

                DropComponent comp = e.GetComponentData<DropComponent>();
                if (comp.IsInPickUp){
                    if (sqrDist < 1f){
                        ItemRewardUitl.OnPickUpItem(player, e);
                        EcsWorld.KillEntity(e);
                    }
                    continue;
                }

                if (sqrDist > pickUpRange)
                    continue;

                comp.IsInPickUp = true;
                LogicDataUtils.SetPickUpMove(e, player);
            }
        }
    }
}
