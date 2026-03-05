

using System.Collections.Generic;

namespace PlayerSystemData
{
    public class MapRankSystem : BaseDataSystem
    {
        #region
        public static MapRankSystem Singleton{
            get{
                if (null == s_singleton) s_singleton = new MapRankSystem();
                return s_singleton;
            }
        }
        static MapRankSystem s_singleton;
        #endregion

        public MapRankData MapRankData { get { return (MapRankData)systemData; } }
        public override string SystemName { get { return "MapRankSystem"; } }
        public MapRankSystem(){
            systemData = new MapRankData();
        }

        public void SetMapPass(int mapId, int rankId){
            int index = mapId * 100 + rankId;
            MapRankData.MapPass.TryAdd(index, true);
            SystemUtils.SetDataDirty();
        }

        public bool GetMapPass(int rankIndex){
            if (rankIndex == 0) return true;
            return MapRankData.MapPass.GetValueOrDefault(rankIndex, false);
        }
    }
}