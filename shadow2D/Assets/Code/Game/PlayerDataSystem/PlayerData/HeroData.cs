using ECS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlayerSystemData
{
    public class HeroInfo {
        public int heroId = 0;
        public bool Lock = false;
    }


    public class HeroData : BaseSaveData{
        public Dictionary<int, HeroInfo> heros = new Dictionary<int, HeroInfo>();

        public override bool SerializeData(out string SaveStr){
            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            SerializeValue(saveStrMap, "heros", heros);
            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override bool DeserializeData(string LoadStr){
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);
            heros = DeserializeValue(saveStrMap, "heros", heros);
            return true;
        }
    }
}
