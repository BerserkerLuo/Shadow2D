using ECS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlayerSystemData
{
    public class WeaponInfo {
        public int weaponId = 0;
        public bool Lock = false;
    }


    public class WeaponData : BaseSaveData{
        public Dictionary<int, WeaponInfo> weapons = new Dictionary<int, WeaponInfo>();

        public override bool SerializeData(out string SaveStr){
            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            SerializeValue(saveStrMap, "weapons", weapons);
            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override bool DeserializeData(string LoadStr){
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);
            weapons = DeserializeValue(saveStrMap, "weapons", weapons);
            return true;
        }
    }
}
