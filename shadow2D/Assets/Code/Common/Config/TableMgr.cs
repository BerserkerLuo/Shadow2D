using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using UnityEngine;

namespace Table
{
    public class TableMgr : SingletonBase<TableMgr>
    {
        public Tables tables = null;

        public TableMgr() { }

        public void LoadCfg() {
            tables = new Tables(file => JSON.Parse(File.ReadAllText($"{Application.streamingAssetsPath}/{"JsonData"}/{file}.json")));
        }

        public static string GetGlobalStrValue(string key,string defv) {
            var cfg = Singleton.tables.GlobalDefCfgMgr.GetOrDefault(key);
            if (cfg == null) return defv;
            return cfg.Value;
        }

        public static int GetGlobalIntValue(string key, int defv) {
            var cfg = Singleton.tables.GlobalDefCfgMgr.GetOrDefault(key);
            if (cfg == null) return defv;
            return int.Parse(cfg.Value);
        }

        public BulletCfg GetBulletCfg(int key) { return tables.BulletCfgMgr.GetOrDefault(key); }
        public HeroCfg GetHeroCfg(int key) { return tables.HeroCfgMgr.GetOrDefault(key); }
        public ItemCfg GetItemCfg(int key) { return tables.ItemCfgMgr.GetOrDefault(key); }
        public ItemBattleCfg GetItemBattleCfg(int key) { return tables.ItemBattleCfgMgr.GetOrDefault(key); }
        public PrefabsEffectCfg GetPrefabsEffectCfg(string key) { return tables.PrefabsEffectCfgMgr.GetOrDefault(key); }
        public ProfebsModelCfg GetProfebsModelCfg(string key) { return tables.ProfebsModelCfgMgr.GetOrDefault(key); }
        public SkillCfg GetSkillCfg(int key) { return tables.SkillCfgMgr.GetOrDefault(key); }
        public SkillLvCfg GetSkillLvCfg(int key,int subKey) { return tables.SkillLvCfgMgr.GetOrDefault(key * 100 + subKey); }
        public MonsterCfg GetMonsterCfg(int key) { return tables.MonsterCfgMgr.GetOrDefault(key); }
        public StatusCfg GetStatusCfg(int key) { return tables.StatusCfgMgr.GetOrDefault(key); }
        public AttrConvertCfg GetAttrConvertCfg(int key) { return tables.AttrConvertCfgMgr.GetOrDefault(key); }
        public DialogueCfg GetDialogueCfg(int key) { return tables.DialogueCfgMgr.GetOrDefault(key); }
        public MapCfg GetMapCfg(int key){ return tables.MapCfgMgr.GetOrDefault(key); }
        public WeaponCfg GetWeaponCfg(int key) { return tables.WeaponCfgMgr.GetOrDefault(key); }
        public MapRankCfg GetMapRankCfg(int key,int subKey) { return tables.MapRankCfgMgr.GetOrDefault(key*100+subKey); }

    }
} 
