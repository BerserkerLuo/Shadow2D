
using Client.UI;
using System.Collections.Generic;
using Table;
namespace PlayerSystemData
{

    //商店
    static class ShopUtils
    {
        public static void BuyHero(int heroId) {
            HeroCfg heroCfg = TableMgr.Singleton.GetHeroCfg(heroId);
            if (heroCfg == null)
                return;

            if (HeroSystem.Singleton.CheckHeroIsLock(heroId))
                return;

            if (!CurrencySystem.Singleton.ChangeGold(-heroCfg.UnLock)) {
                DlgMessage.singleton.ShowMessage("货币不够!");
                return;
            }

            HeroSystem.Singleton.UnLockHero(heroId);
            DlgStartControl.singleton.RefreshCoin();
            DlgSelectHero.singleton.OnShowHeroList();
        }

        public static void BuyWeapon(int weaponId)
        {
            WeaponCfg weaponCfg = TableMgr.Singleton.GetWeaponCfg(weaponId);
            if (weaponCfg == null)
                return;

            if (WeaponSystem.Singleton.CheckWeaponIsLock(weaponId))
                return;

            if (!CurrencySystem.Singleton.ChangeGold(-weaponCfg.UnLock))
            {
                DlgMessage.singleton.ShowMessage("货币不够!");
                return;
            }

            WeaponSystem.Singleton.UnLockWeapon(weaponId);
            DlgStartControl.singleton.RefreshCoin();
            DlgSelectWeapon.singleton.ShowWeaponList();
        }
    }

}


