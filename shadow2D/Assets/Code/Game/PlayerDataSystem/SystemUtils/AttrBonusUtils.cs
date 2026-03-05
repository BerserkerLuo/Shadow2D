using Table;
using Game;
using System.Collections.Generic;

namespace PlayerSystemData
{
    enum LevelChangeType
    {
        Upgrade = 1,
        ReduceLevel = 2,
    }

    static class AttrBonusUtils
    {

        public static bool TryChangeAttrLevel(int attrType, int changeValue)
        {
            AttrBonusSystem system = SystemUtils.GetAttrBonusSystem();
            if (system == null)
                return false;

            return system.TryChangeAttrLevel(attrType, changeValue);
        }

        static public AttrLevelInfo GetAttrInfoByAttrType(int attrType)
        {
            AttrBonusSystem system = SystemUtils.GetAttrBonusSystem();
            if (system == null)
                return null;

            AttrLevelInfo info = system.GetAttrInfo(attrType);
            return info;
        }

        //判断是否能够升级属性
        public static bool AttrLevelChange(int attrType, int level, int type = 1)
        {
            //int curLevel = 0;
            //bool isSuccess = false;
            ////有多少钱
            //int CurrencyNum = CurrencyUtils.GetCurrencyByType(type);

            //AttrLevelInfo attrLevelInfo = GetAttrInfoByAttrType(attrType);
            //if (attrLevelInfo != null)
            //    curLevel = attrLevelInfo.attrLevel;
            ////降级
            //if (curLevel == level + 1)
            //{
            //    DataAttrLevel dataAttr = GetDataAttrLevel(attrType, level + 1);
            //    if (dataAttr == null)
            //        return false;
            //    int CoinNum = dataAttr.UseItemNumList[0];
            //    //这边强制写的0，就是玩家升级的次数产生的变化数
            //    int playerItemNum = GetExtraCoinNum(0,(int)LevelChangeType.ReduceLevel);
            //    CoinNum += playerItemNum;
            //    if (CoinNum > CurrencyNum)
            //        return false;
            //    isSuccess = CurrencyUtils.TryChangeCurrency(type,CoinNum);
            //}
            //else  //升级
            //{
            //    DataAttrLevel dataAttr = GetDataAttrLevel(attrType, level);
            //    if (dataAttr == null)
            //        return false;
            //    int CoinNum = dataAttr.UseItemNumList[0];
            //    //这边强制写的0，就是玩家升级的次数产生的变化数
            //    int playerItemNum = GetExtraCoinNum(0, (int)LevelChangeType.Upgrade);
            //    CoinNum += playerItemNum;
            //    if (CoinNum > CurrencyNum)
            //        return false;
            //    isSuccess = CurrencyUtils.TryChangeCurrency(type, -CoinNum);
            //}
            //if (isSuccess)
            //{
            //    TryChangeAttrLevel(attrType, level);
            //    return true;
            //}
                
            return false;
            
        }

        //是否成功消耗金币
        //private static bool isSuccessChangeCoinNum(int attrType, int level,int type = 1)
        //{
        //    //有多少钱
        //    int CurrencyNum = CurrencyUtils.GetCurrencyByType(type);

        //    DataAttrLevel dataAttr = GetDataAttrLevel(attrType, level);
        //    if (dataAttr == null)
        //        return false;
        //    int CoinNum = dataAttr.UseItemNumList[0];
        //    //这边强制写的0，就是玩家升级的次数产生的变化数
        //    int ExtraItemNum = GetExtraCoinNum(0, (int)LevelChangeType.ReduceLevel);
        //    CoinNum += ExtraItemNum;
        //    if (CoinNum > CurrencyNum)
        //        return false;
        //    bool isSuccess = CurrencyUtils.TryChangeCurrency(type, -CoinNum);
        //    return isSuccess;
        //}

        //取对应属性的等级对应的数值
        //public static DataAttrLevel GetDataAttrLevel(int type, int level)
        //{
        //    DataAttrLevel dataAttr = null;
        //    List<DataAttrLevel> dataAttrs = DataAttrLevelManager.Instance.GetDataListByType(type);
        //    if (dataAttrs.Count == 0)
        //        return null;
        //    foreach (DataAttrLevel attr in dataAttrs)
        //    {
        //        if (attr.Level == level || level == (attr.Level + 1))
        //        {
        //            dataAttr = attr;
        //        }
        //    }
        //    return dataAttr;
        //}

        //public static int GetExtraCoinNum(int attrType,int type)
        //{
        //    int ExtraItemNum = 0;
        //    List<DataAttrLevel> attrLevelsList = DataAttrLevelManager.Instance.GetDataListByType(attrType);
        //    if (attrLevelsList.Count == 0)
        //        return 0;
        //    int UpgradeTimes = GetAttrLevelTimes();
        //    if (type == (int)LevelChangeType.ReduceLevel)
        //    {
        //        UpgradeTimes -= 1;
        //    }
        //    for (int i = 0; i < attrLevelsList.Count; i++)
        //    {
        //        if (UpgradeTimes == i)
        //        {
        //            ExtraItemNum = attrLevelsList[i].UseItemNumList[0];
        //        }
        //    }
        //    return ExtraItemNum;
        //}

        //玩家等级对应的道具数量
        //public static int GetUpgradeCoinNum(int attrType)
        //{
        //    int ExtraItemNum = 0;
        //    List<DataAttrLevel> attrLevelsList = DataAttrLevelManager.Instance.GetDataListByType(attrType);
        //    if (attrLevelsList.Count == 0)
        //        return 0;
        //    int UpgradeTimes = GetAttrLevelTimes();
        //    for (int i = 0; i < attrLevelsList.Count; i++)
        //    {
        //        if (UpgradeTimes == i)
        //        {
        //            ExtraItemNum = attrLevelsList[i].UseItemNumList[0];
        //        }
        //    }
        //    return ExtraItemNum;   
        //}

        //获取属性的升级数量
        public static int  GetAttrLevelTimes()
        {
            int ItemNum = 0;
            var itemList = GetAttrInfoList();
            if (itemList.Count == 0)
                return 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                ItemNum += itemList[i].attrLevel; 
            }
            return ItemNum;
        }

        //属性的对应信息
        public static List<AttrLevelInfo> GetAttrInfoList()
        {
            AttrBonusSystem attrSys = SystemUtils.GetAttrBonusSystem();
            if (attrSys == null)
                return null;
            List<AttrLevelInfo> attrInfoList = attrSys.GetAttrInfoList();
            if (attrInfoList == null)
                return null;
            return attrInfoList;
        }

        ////获取每个属性升级需要花费的道具数量
        //public static int GetAttrLevelByCoinNum(int attrId, int level)
        //{
        //    int itemNum = 0;
        //    DataAttrLevel dataAttrLevel = GetDataAttrLevel(attrId,level);
        //    if (dataAttrLevel == null)
        //        return itemNum;
        //    itemNum = dataAttrLevel.UseItemNumList[0];
        //    int playerItemNum = GetExtraCoinNum(0, (int)LevelChangeType.Upgrade);
        //    return itemNum + playerItemNum;
        //}

    }
}
