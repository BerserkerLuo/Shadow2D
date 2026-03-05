using System;
using System.Collections.Generic;

namespace PlayerSystemData
{
    public class OperationSystem : BaseDataSystem{
        #region
		public static OperationSystem Singleton{
            get{
				if (null == s_singleton)s_singleton = new OperationSystem();
                return s_singleton;
            }
        }
        static OperationSystem s_singleton;
        #endregion
        public override string SystemName { get { return "OperationSystem"; } }
        public OperationData OperationData { get { return (OperationData)systemData; } }
        public OperationSystem(){
            systemData = new OperationData();
        }

        //============================================================================
        public void SetHeroId(int heroId) {OperationData.selectHeroId = heroId;}
        public void SetWeaponId(int weaponId) { OperationData.seltctWeaponId = weaponId; }
        public void SetMapId(int mapId) { OperationData.selectMapId = mapId; }
        public void SetMapRank(int mapId,int mapRank) {if (!OperationData.selectMapRank.TryAdd(mapId, mapRank)) OperationData.selectMapRank[mapId] = mapRank;}

        //============================================================================
        public int GetHeroId() { return OperationData.selectHeroId; }
        public int GetWeaponId() { return OperationData.seltctWeaponId; }
        public int GetMapId() { return OperationData.selectMapId; }
        public int GetMapRank(int mapId) { return OperationData.selectMapRank.GetValueOrDefault(mapId,1); }
    }
}