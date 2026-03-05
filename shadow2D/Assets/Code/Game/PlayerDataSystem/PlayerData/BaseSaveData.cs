using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerSystemData
{
    public interface IBaseSaveData {
        public bool SerializeData(out string SaveStr);
        public bool DeserializeData(string LoadStr);
    }

    public class BaseSaveData : IBaseSaveData
    {
        public virtual bool SerializeData(out string SaveStr) { SaveStr = ""; return true; }
        public virtual bool DeserializeData(string LoadStr) { return true;}

        public void SerializeValue<T>(Dictionary<string, string> saveStrMap, string name,T value) {
            saveStrMap.Add(name,JsonConvert.SerializeObject(value));
        }
        public T DeserializeValue<T>(Dictionary<string, string> saveStrMap,string name,T defaultvalue) {
            string tempStr = saveStrMap.GetValueOrDefault(name,"");
            if (tempStr == "")  return defaultvalue;
            return JsonConvert.DeserializeObject<T>(tempStr);
        }

        
    }
}
