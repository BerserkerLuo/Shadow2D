using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerSystemData
{

    public class OperationData : BaseSaveData
    {
        public int selectHeroId = 0;
        public int seltctWeaponId = 0;
        public int selectMapId = 0;
        public Dictionary<int, int> selectMapRank = new Dictionary<int, int>();

        public OperationData() { }

        public override bool SerializeData(out string SaveStr) {
            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            SerializeValue(saveStrMap, "selectMapId", selectMapId);
            SerializeValue(saveStrMap, "selectMapRank", selectMapRank);
            SerializeValue(saveStrMap, "selectHeroId", selectHeroId);
            SerializeValue(saveStrMap, "seltctWeaponId", seltctWeaponId);
            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override  bool DeserializeData(string LoadStr){
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);
            selectMapId = DeserializeValue(saveStrMap, "selectMapId", selectMapId);
            selectMapRank = DeserializeValue(saveStrMap, "selectMapRank", selectMapRank);
            selectHeroId = DeserializeValue(saveStrMap, "selectHeroId", selectHeroId);
            seltctWeaponId = DeserializeValue(saveStrMap, "seltctWeaponId", seltctWeaponId);
            return true;
        }
    }
}
