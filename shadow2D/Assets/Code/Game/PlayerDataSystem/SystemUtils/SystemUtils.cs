


namespace PlayerSystemData
{
    public class SystemUtils 
    {
        public static void SetDataDirty() {SystemMgr.Singleton.isDirt = true;}

        public static MapRankSystem GetMapRankSystem() { return (MapRankSystem)SystemMgr.Singleton.GetSystem("MapRankSystem"); }

        public static BackpackSystem GetBackpackSystem() { return (BackpackSystem)SystemMgr.Singleton.GetSystem("BackpackSystem"); }

        public static CurrencySystem GetCurrencySystem() { return (CurrencySystem)SystemMgr.Singleton.GetSystem("CurrencySystem"); }

        public static HeroSystem GetHeroSystem() { return (HeroSystem)SystemMgr.Singleton.GetSystem("HeroSystem"); }

        public static AttrBonusSystem GetAttrBonusSystem() { return (AttrBonusSystem)SystemMgr.Singleton.GetSystem("AttrBonusSystem"); }

        public static OperationSystem GetOperationSystem() { return (OperationSystem)SystemMgr.Singleton.GetSystem("OperationSystem"); }
    }
}
