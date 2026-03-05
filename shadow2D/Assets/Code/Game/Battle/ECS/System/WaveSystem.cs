
using Table;
using UnityEngine;

namespace ECS
{
    class WaveSystem : System
    {
        public WaveSystem(ECSWorld world)
        {
            Init(world);
        }

        public override void Update(){
            WaveComponent comp = EcsWorld.GlobalEntity.GetComponentData<WaveComponent>();

            WaveUtil.TryActiveWave(EcsWorld, comp);
            WaveUtil.TryEndWave(EcsWorld, comp);

            CreteMonster(comp);
        }

        public void CreteMonster(WaveComponent comp) {

            float now = EcsWorld.Time;

            foreach (var it in comp.ActiveWaves) {
                WaveInfo waveInfo = it.Value;
                if (waveInfo.nextRefreshTime > now)
                    continue;

                waveInfo.nextRefreshTime += waveInfo.waveCfg.Interval;

                WaveCfg waveCfg = waveInfo.waveCfg;
                for (int i = 0; i < waveCfg.RefreshCount; ++i) {
                    Vector3 pos = LogicUtils.GetSurroundPlayerPos(EcsWorld, 40, 32);
                    CreateUtil.CreateMonster(EcsWorld, pos, waveCfg.MonsterId);
                }
            }  
        }
    }
}
