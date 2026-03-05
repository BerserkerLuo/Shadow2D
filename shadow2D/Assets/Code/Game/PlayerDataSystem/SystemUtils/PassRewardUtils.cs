
using System.Collections.Generic;

namespace PlayerSystemData
{
    public class PassRewardUtils
    {
        //private static void SetMapPass(int mapId, int rankId)
        //{
        //    MapRankSystem mapRankSys = SystemUtils.GetMapRankSystem();
        //    if (mapRankSys == null)
        //        return;

        //    mapRankSys.SetMapPass(mapId, rankId);
        //}

        public static string GivePassReward(int mapId, int rankId, string originReward)
        {
            return "";
            //MapRankSystem mapRankSys = SystemUtils.GetMapRankSystem();
            //if (mapRankSys == null)
            //    return originReward;

            //DataMap mapCfg = DataMapManager.Instance.GetDataByID(mapId);
            //if (mapCfg == null)
            //    return originReward;

            //var originRewardDic = CommonUtils.GetRewardItems(originReward);

            //Dictionary<int, int> passRankRewardDic = null;
            //Dictionary<int, int> passMapRewardDic = null;

            ////有难度等级
            //if (mapCfg.MaxRank != 0)
            //{
            //    List<DataMapwave> listWave = DataMapwaveManager.Instance.GetDataListByMapId(mapId);
            //    foreach (var it in listWave)
            //    {
            //        if (it.Level != rankId)
            //            continue;

            //        //重复通关该难度
            //        if (mapRankSys.GetMapPass(mapId, rankId))
            //            passRankRewardDic = CommonUtils.GetRewardItems(it.RepeatPassReward);
            //        else
            //        {
            //            //首次通关该难度
            //            SetMapPass(mapId, rankId);
            //            passRankRewardDic = CommonUtils.GetRewardItems(it.FirstPassReward);
            //        }
            //    }

            //    //首次通关所有难度
            //    if(rankId == mapCfg.MaxRank && !mapRankSys.GetMapPass(mapId, 0))
            //    {
            //        SetMapPass(mapId, 0);
            //        passMapRewardDic = CommonUtils.GetRewardItems(mapCfg.FirstPassReward);
            //    }
            //}
            //else
            //{
            //    if(mapRankSys.GetMapPass(mapId, 0))
            //        passMapRewardDic = CommonUtils.GetRewardItems(mapCfg.RepeatPassReward);
            //    else
            //    {
            //        SetMapPass(mapId, 0);
            //        passMapRewardDic = CommonUtils.GetRewardItems(mapCfg.FirstPassReward);
            //    }
            //}


            //var tmpDic = AddDic(originRewardDic, passRankRewardDic);
            //var finalDic = AddDic(tmpDic, passMapRewardDic);

            //return CommonUtils.GetRewardStr(finalDic);
        }

        //private static Dictionary<int,int> AddDic(Dictionary<int, int> d1, Dictionary<int, int> d2)
        //{
        //    if(d1 == null && d2 == null)
        //        return null;

        //    if (d1 == null)
        //        return d2;

        //    if(d2 == null)
        //        return d1;

        //    if (d1.Count == 0)
        //        return d2;

        //    if (d2.Count == 0)
        //        return d1;

        //    foreach (var pair in d2)
        //    {
        //        d1.TryGetValue(pair.Key, out int v);
        //        d1[pair.Key] = pair.Value + v;
        //    }

        //    return d1;
        //}
    }
}