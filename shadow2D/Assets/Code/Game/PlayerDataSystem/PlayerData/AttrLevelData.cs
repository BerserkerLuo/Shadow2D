using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlayerSystemData
{
    public class AttrLevelInfo
    {
        public int attrLevel = 0;
        public int attrType = 0;
    }

    public  class AttrLevelData : BaseSaveData
    {
        public Dictionary<int, AttrLevelInfo> attrs = new Dictionary<int, AttrLevelInfo>();

        public override bool SerializeData(out string SaveStr)
        {
            string itemsStr = JsonConvert.SerializeObject(attrs);

            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            saveStrMap.Add("attrsStr", itemsStr);

            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override bool DeserializeData(string LoadStr)
        {
            string tempStr = "";
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);

            if (saveStrMap.TryGetValue("attrsStr", out tempStr) == true)
                attrs = JsonConvert.DeserializeObject<Dictionary<int, AttrLevelInfo>>(tempStr);

            return true;
        }
    }
}
