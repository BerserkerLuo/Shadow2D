
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerSystemData
{
    public enum ItemType {
        NoreMal     = 1, //普通道具 
        Equip       = 2, //装备
    }

    public enum EquipType { 
        Weapon = 1, //武器
        Armor = 2,  //防具
        Accessories = 3, //饰品
    }

    public enum EquipPort {
        Error = 0,  //错误
        Weapon = 1, //武器
        Armor1 = 2, //防具1
        Armor2 = 3, //防具2
        Annex1 = 4, //饰品1
        Annex2 = 5, //饰品2
        Annex3 = 6, //饰品3
    }

    public class ItemInfo{
        public int itemXmlId = 0;
        public int itemCount = 0;
        public int expireTime = 0;
    }

    public class BackpackData : BaseSaveData
    {
        public Dictionary<int, ItemInfo> items = new Dictionary<int, ItemInfo>();

        public override bool SerializeData(out string SaveStr) {
            string itemsStr = JsonConvert.SerializeObject(items);

            Dictionary<string, string> saveStrMap = new Dictionary<string, string>();
            saveStrMap.Add("itemsStr", itemsStr);

            SaveStr = JsonConvert.SerializeObject(saveStrMap);
            return true;
        }

        public override bool DeserializeData(string LoadStr) {
            string tempStr = "";
            Dictionary<string, string> saveStrMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);

            if(saveStrMap.TryGetValue("itemsStr", out tempStr) == true)
                items = JsonConvert.DeserializeObject<Dictionary<int, ItemInfo>>(tempStr);

            return true;
        }
    }
}
