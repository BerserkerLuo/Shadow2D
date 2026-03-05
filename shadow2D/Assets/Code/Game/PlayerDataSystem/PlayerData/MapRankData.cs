
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerSystemData
{


    public class MapRankData : BaseSaveData
    {
        public Dictionary<int,bool> MapPass = new Dictionary<int, bool>();

        public override bool SerializeData(out string SaveStr) {
            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            SerializeValue(saveStrMap, "MapPass", MapPass);
            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override bool DeserializeData(string LoadStr) {
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);
            DeserializeValue(saveStrMap, "MapPass", MapPass);
            return true;
        }
    }
}
