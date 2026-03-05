using Table;
using System;
namespace Tool
{
    public class GlobalCfg
    {
        static public int GetLevelUpDropId(){ 
            return TableMgr.GetGlobalIntValue("LevelUpDropId", 101);
        }
        static public int GetWeaponShopDropId(){
            return TableMgr.GetGlobalIntValue("WeaponShopDropId", 102);
        }
    }
}
