
using System.Collections.Generic;
using Table;
using UnityEngine;

namespace ECS
{
    internal class WaveUtil
    {
        public static void InitWave(ECSWorld world) {
            WaveComponent comp = world.GlobalEntity.GetComponentData<WaveComponent>();
            List<WaveCfg> cfgList = TableMgr.Singleton.tables.WaveCfgMgr.DataList;

            int MapId = MapUtil.GetMapId(world);
            int RankId = MapUtil.GetMapRank(world);

            MapRankCfg mapRankCfg = TableMgr.Singleton.GetMapRankCfg(MapId,RankId);
            int waveGroup = mapRankCfg.WaveGroup;

            foreach (WaveCfg waveCfg in cfgList) {
                if (waveCfg.Group != waveGroup)
                    continue;
                comp.waveCfgs.Add(waveCfg);
            }

            TryActiveWave(world,comp);
        }

        public static void TryActiveWave(ECSWorld world,WaveComponent comp) {
            float gameTime = world.Time;
            foreach (WaveCfg waveCfg in comp.waveCfgs) {
                if (waveCfg.StartTime > gameTime || waveCfg.EndTime < gameTime)
                    continue;
                if (comp.ActiveWaves.ContainsKey(waveCfg.ID))
                    continue;

                Debug.Log($"ActiveWave {waveCfg.ID}");

                comp.ActiveWaves.Add(waveCfg.ID,new WaveInfo(waveCfg));
            }
        }

        static List<int> RevmoeList = new List<int>();
        public static void TryEndWave(ECSWorld world, WaveComponent comp) {
            float gameTime = world.Time;
            foreach (var it in comp.ActiveWaves){
                WaveInfo waveInfo = it.Value;
                if (waveInfo.waveCfg.EndTime > gameTime)
                    continue;
                RevmoeList.Add(it.Key);
            }

            if (RevmoeList.Count == 0)
                return;

            foreach (int Id in RevmoeList)
                comp.ActiveWaves.Remove(Id);
            RevmoeList.Clear();
        }

        public static bool CheckAllWaveEnd(ECSWorld world) {
            float gameTime = world.Time;
            WaveComponent comp = world.GlobalEntity.GetComponentData<WaveComponent>();
            foreach (WaveCfg waveCfg in comp.waveCfgs){
                if (waveCfg.EndTime > gameTime)
                    return false;
            }
            return true;
        }
    }
}
