using System.Collections;
using UnityEngine;
using Table;
using System.Collections.Generic;

namespace ECS
{
    public class BackpackUtil{
        public static Dictionary<int, int> GetBackpackDic(Entity le){
            var backpackCom = le.GetComponentData<BackpackComponent>();
            if (backpackCom == null)
                return null;
            return backpackCom.BackpackDic;
        }

        public static void AddIntoBackpack(Entity le, int itemId, int itemNum) {
            if (itemNum <= 0)
                return;

            var BackpackDic = GetBackpackDic(le);
            if (BackpackDic == null)
                return;

            int tmpNum = BackpackDic.GetValueOrDefault(itemId,0);
            BackpackDic[itemId] = tmpNum + itemNum;
        }

        public static int GetItemNum(Entity le, int itemId) {
            var BackpackDic = GetBackpackDic(le);
            if (BackpackDic == null)
                return 0;
            return BackpackDic.GetValueOrDefault(itemId, 0);
        }

        public static int GetCoinCount(Entity le) {
            return GetItemNum(le, ItemId.CoinId);
        }
    }
}