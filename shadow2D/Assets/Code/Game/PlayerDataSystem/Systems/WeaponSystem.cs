
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerSystemData
{
    public class WeaponSystem : BaseDataSystem{
        //单例
        #region
        public static WeaponSystem Singleton{
            get{
                if (null == s_singleton) s_singleton = new WeaponSystem();
                return s_singleton;
            }
        }
        static WeaponSystem s_singleton;
        #endregion

        public override string SystemName { get { return "WeaponSystem"; } }
        public WeaponData WeaponData { get { return (WeaponData)systemData; } }
        public WeaponSystem(){
            systemData = new WeaponData();
        }

        public void UnLockWeapon(int weaponId) {
            WeaponInfo weaponInfo = WeaponData.weapons.GetValueOrDefault(weaponId,null);
            if (weaponInfo == null) {
                weaponInfo = new WeaponInfo();
                WeaponData.weapons.Add(weaponId, weaponInfo);
            }
            weaponInfo.Lock = true;
        }

        public bool CheckWeaponIsLock(int weaponId) {
            WeaponInfo weaponInfo = WeaponData.weapons.GetValueOrDefault(weaponId, null);
            if (weaponInfo == null)
                return false;
            return weaponInfo.Lock;
        }
    }
}
