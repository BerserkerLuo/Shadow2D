
using System.Collections.Generic;
using Table;
using UnityEngine;

namespace ECS
{
    public class WaveInfo {
        public WaveInfo(WaveCfg cfg) {
            waveId = cfg.ID;
            waveCfg = cfg;
            nextRefreshTime = Time.time;
        }
        public int waveId;
        public WaveCfg waveCfg;
        public float nextRefreshTime;
    }
    
    //波次组件
    public class WaveComponent : Component{
        public Dictionary<int,WaveInfo> ActiveWaves = new();
        public List<WaveCfg> waveCfgs = new List<WaveCfg>();
    }
}
