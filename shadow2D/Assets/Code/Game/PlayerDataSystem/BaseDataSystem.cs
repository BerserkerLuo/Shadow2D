

using System;
using UnityEngine;

namespace PlayerSystemData
{
    public interface IBaseDataSystem
    {
        string SystemName { get; }
        void Update(float frameTime);

        bool LoadData(string JsonData);

        void AfterLoadData();

        bool SaveData(out string SaveJson);
    }

    public class BaseDataSystem : IBaseDataSystem{

        public virtual string SystemName { get { return "BaseSystem"; } }

        protected BaseSaveData systemData = null;

        public BaseDataSystem(){}

        public virtual void Update(float frameTime) {
            
        }

        public virtual bool LoadData(string JsonData){
            try{
                if (systemData != null) systemData.DeserializeData(JsonData);
                return true;
            }
            catch( Exception e) {
                Debug.LogError(e);
                return false;
            }
        }

        public virtual void AfterLoadData()
        {

        }

        public virtual bool SaveData(out string SaveJson) {
            try {
                if (systemData != null)
                    systemData.SerializeData(out SaveJson);
                else
                    SaveJson = "";
                return true;
            }
            catch (Exception e) {
                Debug.LogError(e);
                SaveJson = "";
                return false;
            }
        }
    }

   
}
