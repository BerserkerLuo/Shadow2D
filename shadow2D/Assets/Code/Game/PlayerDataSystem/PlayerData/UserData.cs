
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerSystemData
{

    public class UserData : BaseSaveData
    {
        public int level = 1;
        public int powerNum = 0;        //体力
        public float expNum = 0;        //经验

        public UserData() { }

        public override bool SerializeData(out string SaveStr) {
            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            SerializeValue(saveStrMap, "level",level);
            SerializeValue(saveStrMap, "powerNum", powerNum);
            SerializeValue(saveStrMap, "expNum", expNum);
            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override bool DeserializeData(string LoadStr) {
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);
            level = DeserializeValue(saveStrMap, "level", level);
            powerNum = DeserializeValue(saveStrMap, "powerNum", powerNum);
            expNum = DeserializeValue(saveStrMap, "expNum", expNum);
            return true;
        }
    }
}
