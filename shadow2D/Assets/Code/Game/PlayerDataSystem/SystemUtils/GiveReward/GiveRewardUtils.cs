
using Table;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystemData
{
    static class GiveRewardUtils {

        //给物品奖励
        static public void GiveItemReward(string rewardStr) { giveItemReward(rewardStr); }

        static public void GiveItemReward(Dictionary<int, int> rewardMap) { giveItemReward(rewardMap); }

        //给物品使用的奖励
        static public void GiveItemUseReward(string rewardStr) { giveItemReward(rewardStr, true); }



        private static void giveItemReward(string rewardStr, bool isUse = false)
        {
            if (rewardStr == null || rewardStr == "")
                return;

            Dictionary<int, int> rewardMap = CommonUtils.GetRewardItems(rewardStr);

            giveItemReward(rewardMap,isUse);
        }

        static private void giveItemReward(Dictionary<int, int> rewardMap, bool isUse = false)
        {
            if (rewardMap == null || rewardMap.Count == 0)
                return;

            Dictionary<int, int> directEnterPackItems = new Dictionary<int, int>();

            // 1 筛选出直接进背包的道具
            splitFlowItem(rewardMap,directEnterPackItems,isUse);

            // 2 执行掉落
            ExcuteDrop(rewardMap);

            // 3 筛选出直接进背包的道具
            splitFlowItem(rewardMap, directEnterPackItems, isUse);

            // 4 执行给非背包道具流程
            excuteGiveNoIntoPackItems(rewardMap);

            // 5 把剩余道具和之前筛选出的直接进背包的道具合并
            foreach (var it in rewardMap) {
                if (directEnterPackItems.ContainsKey(it.Key) == false) directEnterPackItems[it.Key] = 0;
                directEnterPackItems[it.Key] += it.Value;
            }

            // 6 执行给背包道具流程
            excuteGiveIntoPackItems(directEnterPackItems);


            // 7 给玩家显示获取的奖励
        }

        static List<int> removeList = new List<int>();
        static private void splitFlowItem(Dictionary<int, int> rewardMap, Dictionary<int, int> directEnterPackItems,bool isUse) {

            //if (isUse) return;

            //removeList.Clear();
            //foreach (var it in rewardMap) {
            //    int itemId = it.Key;
            //    int itemCount = it.Value;
            //    DataItem itemCfg = DataItemManager.Instance.GetDataByID(itemId);
            //    if (itemCfg == null) {
            //        removeList.Add(itemId);
            //        continue;
            //    }

            //    //判断是否立即使用  0为不（放入背包）  1为使用 不放背包
            //    if (itemCfg.Canuseimmediately)
            //        continue;
                
            //    if (directEnterPackItems.ContainsKey(itemId) == false) directEnterPackItems[itemId] = 0;
            //    directEnterPackItems[itemId] += itemCount;

            //    removeList.Add(itemId);
            //}

            //foreach (var it in removeList)
            //    rewardMap.Remove(it);
        }

        //执行掉落
        static private void ExcuteDrop(Dictionary<int, int> rewardMap) {
            //to do ...
        }

        //给非背包道具
        static private void excuteGiveNoIntoPackItems(Dictionary<int, int> rewardMap){
            List<int> removeList = new List<int>();
            foreach (var it in rewardMap){
                if (excuteGiveNoIntoPackItem(it.Key, it.Value) == false)
                    continue;

                removeList.Add(it.Key);
            }

            foreach (var it in removeList)  rewardMap.Remove(it);
        }
        
        //给背包道具
        static private void excuteGiveIntoPackItems(Dictionary<int, int> rewardMap){
            foreach (var it in rewardMap) {
                int itemId = it.Key;
                int itemCount = it.Value;
                Debug.Log("给背包道具" + itemId + "   " + itemCount);
                BackpakcSystemUtils.TryChangeItemNum(itemId,itemCount);
            }
        }

        //给非背包道具 具体执行
        static bool excuteGiveNoIntoPackItem(int itemId, int itemCount) {
            return false;
            //DataItem itemCfg = DataItemManager.Instance.GetDataByID(itemId);
            //if (itemCfg == null)
            //    return false;

            //BaseGiver rewardGiver = GiverMgr.Singleton.GetRewardGiver(itemCfg.Type);
            //if (rewardGiver == null)
            //    return false;

            //if (rewardGiver.CheckOnlyReward(itemId))
            //{
            //    //发重复补偿 to do...
            //    GiveItemReward(itemCfg.Compensate);

            //}
            //else 
            //    rewardGiver.DoReward(itemId, itemCount);

            //return true;
        }
    }
}