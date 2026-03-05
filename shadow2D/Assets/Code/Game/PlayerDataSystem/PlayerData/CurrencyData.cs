using Newtonsoft.Json;
using System.Collections.Generic;


namespace PlayerSystemData
{
    public class CurrencyData : BaseSaveData
    {
        public Dictionary<int, float> currencyDic = new Dictionary<int, float>();

        public override bool SerializeData(out string SaveStr) {
            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            SerializeValue(saveStrMap, "currencyDic", currencyDic);
            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override bool DeserializeData(string LoadStr) {
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);
            currencyDic = DeserializeValue(saveStrMap, "currencyDic", currencyDic);
            return true;
        }
    }
}
