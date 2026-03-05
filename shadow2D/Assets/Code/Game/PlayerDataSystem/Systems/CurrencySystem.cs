
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystemData
{
    public enum EnumCurrencyType
    {
        Gold = 1,         //金币
        Demond = 2,         //钻石
    }

    public class CurrencySystem :BaseDataSystem
    {
        #region
        public static CurrencySystem Singleton{
            get{
                if (null == s_singleton) s_singleton = new CurrencySystem();
                return s_singleton;
            }
        }
        static CurrencySystem s_singleton;
        #endregion

        CurrencyData CurrencyData = null;
        public override string SystemName { get { return "CurrencySystem"; } }
        public CurrencySystem(){
            CurrencyData = new CurrencyData();
            systemData = CurrencyData;
        }

        //=========================================================================
        private bool ChangeCurrency(int type,int v){
            float curNum = CurrencyData.currencyDic.GetValueOrDefault(type, 0);
            float tarNum = curNum + v;

            Debug.Log($"ChangeCurrency curNum {curNum} v {v}");

            if(tarNum < 0)
                return false;

            CurrencyData.currencyDic[type] = tarNum;

            SystemMgr.SetDirt();
            EventUtils.OnCurrencyChanged(type,v);

            return true;
        }
        public int GetCurrency(int type){
            return (int)CurrencyData.currencyDic.GetValueOrDefault(type, 0);
        }

        public void SetCurrencyNum(int type, int CurrencyNum){
            if (CurrencyData.currencyDic.ContainsKey(type) == false)
                CurrencyData.currencyDic.Add(type, CurrencyNum);
            else
                CurrencyData.currencyDic[type] = CurrencyNum;

            SystemMgr.SetDirt();
        }

        //=========================================================================

        public bool ChangeGold(int value) {
            return ChangeCurrency((int)EnumCurrencyType.Gold, value);
        }
        public int GetGold() {
            return GetCurrency((int)EnumCurrencyType.Gold);
        }


    }
}