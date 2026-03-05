
using System.Collections.Generic;

namespace ECS
{
    class AutoMoveSystem : System
    {
        public AutoMoveSystem(ECSWorld ecsWorld)
        {
            Init(ecsWorld);

            RequireComponent(typeof(AutoMoveComponent));

            AutoMoveDispatcher.ClearTactics();
            AutoMoveDispatcher.RegisterAllBaseMoveFunction();
        }

        public override void Update()
        {
            //if (LogicUtils.IsPause(LogicWorld))
            //    return;

            foreach (var e in entities){

                LogicUtils.BeginSample("TickAutoMove");

                TickAutoMove(e);

                LogicUtils.EndSample();
            }
        }
        private void TickAutoMove(Entity e)
        {
            var com = e.GetComponentData<AutoMoveComponent>();
            if (com == null)
                return;

            TickAutoMove(com);

            //int MoveFlg =  ? 1 : 0;
            //int checkFlag = (com.LastUpdateMove << 1) | MoveFlg;
            //if(checkFlag == 1)
            //    AnimationUtil.Walk(e);
            //else if(checkFlag == 2)
            //    AnimationUtil.StopMove(e);
            //com.LastUpdateMove = MoveFlg;
        }

        private void TickAutoMove(AutoMoveComponent com) {
            AutoMoveParamBase moveParam = com.TempMoveParam;
            if (moveParam == null || moveParam.IsStop)
                moveParam = com.BaseMoveParam;

            if (moveParam == null || moveParam.IsStop)
                return;

            if (moveParam.IsPauseMove) {
                RVOUtil.SetAgentStop(com.Entity);
                return;
            }

            OnAutoMoveFunction moveFun = AutoMoveDispatcher.GetMoveFunction(moveParam.MoveType);
            if (moveFun == null)
                return;

            LogicUtils.BeginSample("autoMove" + moveParam.MoveType.ToString());

            moveFun(com.Entity, moveParam);

            LogicUtils.EndSample();

            TryExpireTempMove(com);
        }

        //处理临时移动的生命周期 (击退,击飞...)
        private void TryExpireTempMove(AutoMoveComponent com) {
            if (com.TempMoveParam == null)
                return;

            float nowTime = LogicUtils.GetTime(com.Entity);
            if (com.TempMoveParam.TempModeEndTime > 0 && com.TempMoveParam.TempModeEndTime < nowTime)
                com.TempMoveParam = null;
        }

    }
}