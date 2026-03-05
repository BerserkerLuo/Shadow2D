using ECS;
using Newtonsoft.Json;
using System.Collections.Generic;
using Tool;
using UnityEngine;
using Table;
using Tools;

namespace PlayerSystemData
{

    public class SystemMgr : SingletonBase<SystemMgr>
    {
        public static void SetDirt() { Singleton.isDirt = true; }

        public bool isDirt = false;
        Dictionary<string, IBaseDataSystem> m_dictSystems = new Dictionary<string, IBaseDataSystem>();

        public SystemMgr()
        {
            RegisterSystem(OperationSystem.Singleton);
            RegisterSystem(WeaponSystem.Singleton);
            RegisterSystem(HeroSystem.Singleton);
            RegisterSystem(MapRankSystem.Singleton);
            RegisterSystem(CurrencySystem.Singleton);
            RegisterSystem(AttrBonusSystem.Singleton);
        }

        public void Init() {

            LoadData();

            TestInit();
        }

        bool isInit = false;
        public void TestInit() {
            if (isInit)
                return;
            isInit = true;


            HeroSystem.Singleton.UnLockHero(1);
            HeroSystem.Singleton.UnLockHero(100);

            //一些测试代码 比如初始给英雄 给钱
            //List<ItemCfg> cfgList = TableMgr.Singleton.tables.ItemCfgMgr.DataList;
            //Dictionary<int, int> rewards = new Dictionary<int, int>();
            //foreach (var cfg in cfgList)
            //{
            //    if (!rewards.ContainsKey(cfg.Id))
            //        rewards.Add(cfg.Indx, 1);
            //    else
            //        Debug.Log("ErrorId " + cfg.Id);
            //}

            //GiveRewardUtils.GiveItemReward(rewards);
        }

        
        //注册系统
        private void RegisterSystem(IBaseDataSystem system) {
            m_dictSystems.Add(system.SystemName,system);
        }

        //获取系统
        private IBaseDataSystem tempSystem;
        public IBaseDataSystem GetSystem(string systemName) {
            tempSystem = null;
            m_dictSystems.TryGetValue(systemName,out tempSystem);
            return tempSystem;
        }

        float lastRefreshTime = 0;
        float lastSaveTime = 0;
        public void Update() {
            //0.1秒调用一次
            float now = Time.time;
            if (now - lastRefreshTime < 0.1f)
                return;
            lastRefreshTime = now;

            foreach (var sysit in m_dictSystems){
                IBaseDataSystem system = sysit.Value;
                system.Update(0.1f);
            }

            if (now - lastSaveTime > 1*60f){
                if (lastSaveTime != 0)
                    SaveData();    
                lastSaveTime = now;
            }
        }
        
        //保存数据
        private Dictionary<string, string> saveMap = new Dictionary<string, string>();
        public void SaveData() {


            DebugUtils.Log("SaveData");

            if (!isDirt)
                return;

            DebugUtils.Log("SaveData Begin");

            saveMap.Clear();

            foreach (var sysit in m_dictSystems) {
                IBaseDataSystem system = sysit.Value;
                string SaveJson = "";
                if (system.SaveData(out SaveJson) == false) { 
                    Debug.LogError("SaveData Error ! SystemName:" + system.SystemName);
                    continue;
                }
                
                saveMap.Add(system.SystemName, SaveJson);
            }

            string SaveStr = JsonConvert.SerializeObject(saveMap);

            DebugUtils.Log("SaveData SaveStr :" + SaveStr);

            SaveStr = Util.CompressString(SaveStr);
            LocalGameDataMgr.Singleton.SetAchieve(SaveStr);
        }

        //加载数据
        public void LoadData(){

            string  LoadStr = Util.DecompressString(LocalGameDataMgr.Singleton.Achieve);

            Dictionary<string, string> LoadMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadStr);
            if (LoadMap == null)
                return;

            foreach (var it in LoadMap){
                IBaseDataSystem sys = GetSystem(it.Key);
                if (sys == null)
                    continue;
                sys.LoadData(it.Value);
            }

            foreach (var it in m_dictSystems) 
                it.Value.AfterLoadData();
        }
    }
}

