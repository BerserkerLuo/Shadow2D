
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    class GameResultSystem : System
    {
        public GameResultSystem(ECSWorld world)
        {
            Init(world);
            
        }

        private float LastCheckTime = 0;

        public override void Update(){
            if (Time.time - LastCheckTime < 0.2f)
                return;
            LastCheckTime = Time.time;

            if (LogicUtils.GetEndGameFlag(EcsWorld))
                return;

            CheckPlayerDead();
            AllMonsterDead();
        }

        public void CheckPlayerDead() {

            Debug.Log("DEVLOG CheckPlayerDead");

            Entity player = LogicUtils.GetPlayerEntity(EcsWorld);
            if (player == null || LogicUtils.IsDead(player))
                GameResultUtil.OnGameEnd(EcsWorld, EndType.PlayerDead);
        }

        public void AllMonsterDead() {
            if (!WaveUtil.CheckAllWaveEnd(EcsWorld))
                return;

            Dictionary<int,Entity> monaterMap = LogicUtils.GetClassMap(EcsWorld,FactionId.Monster);
            if (monaterMap.Count > 0)
                return;

            Debug.Log("DEVLOG AllMonsterDead");

            GameResultUtil.OnGameEnd(EcsWorld, EndType.Win);
        }
    }
}
