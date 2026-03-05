
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerSystemData
{
    public class HeroSystem : BaseDataSystem{
        #region
        public static HeroSystem Singleton{
            get{
                if (null == s_singleton) s_singleton = new HeroSystem();
                return s_singleton;
            }
        }
        static HeroSystem s_singleton;
        #endregion

        public override string SystemName { get { return "HeroSystem"; } }
        public HeroData HeroData { get { return (HeroData)systemData; } }
        public HeroSystem(){
            systemData = new HeroData();
        }

        public void UnLockHero(int heroId) {
            HeroInfo heroInfo = HeroData.heros.GetValueOrDefault(heroId,null);
            if (heroInfo == null) {
                heroInfo = new HeroInfo();
                HeroData.heros.Add(heroId,heroInfo);
            }
            heroInfo.Lock = true;
        }

        public bool CheckHeroIsLock(int heroId) {
            HeroInfo heroInfo = HeroData.heros.GetValueOrDefault(heroId, null);
            if (heroInfo == null)
                return false;
            return heroInfo.Lock;
        }
    }
}
