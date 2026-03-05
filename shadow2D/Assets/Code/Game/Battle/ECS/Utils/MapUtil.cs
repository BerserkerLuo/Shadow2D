using RandMap;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    class MapUtil
    {
        public static MapComponent GetMapComponent(ECSWorld world) {
            return world.GlobalEntity.GetComponentData<MapComponent>();
        }

        public static void InitMapComponent(ECSWorld world,int mapId,int rankId) {
            var MapCreator = GameObject.Find("MapCreator");
            if (MapCreator == null)
                return;

            MapComponent comp = GetMapComponent(world);
            if (comp == null)
                return;

            comp.MapId = mapId;
            comp.RankId = rankId;

            RandMapCreator randMapCreator = MapCreator.GetComponent<RandMapCreator>();
            if (randMapCreator == null)
                return;

            comp.MapWidth = randMapCreator.width;
            comp.MapHeight = randMapCreator.height;
            comp.EdgeWidth = randMapCreator.EdgeWidth;

            comp.PlayerBronArea.startPos = new Vector2Int(2, 2);
            comp.PlayerBronArea.endPos = new Vector2Int(comp.EdgeWidth-1, comp.EdgeWidth-1);
            comp.PlayerBronArea.length = comp.PlayerBronArea.endPos - comp.PlayerBronArea.startPos;

            Area monsterBronArea = new Area();
            monsterBronArea.startPos = new Vector2(comp.MapWidth + comp.EdgeWidth + 1, comp.MapHeight + comp.EdgeWidth + 1);  
            monsterBronArea.endPos = new Vector2(comp.MapWidth + comp.EdgeWidth*2 - 2, comp.MapHeight + comp.EdgeWidth*2 - 2);
            monsterBronArea.length = monsterBronArea.endPos - monsterBronArea.startPos;
            comp.MonsterBronArea.Add(monsterBronArea);

            UIUtils.SetClimpCameraPos(new Vector2(comp.EdgeWidth, comp.EdgeWidth*0.5f),new Vector2(comp.EdgeWidth + comp.MapWidth, comp.EdgeWidth*1.5f + comp.MapHeight));
        }

        public static Vector2 GetRandPos(Area area){
            float x = LogicUtils.GetRand(0, area.length.x) + area.startPos.x;
            float y = LogicUtils.GetRand(0, area.length.y) + area.startPos.y;
            return new Vector2(x, y);
        }

        public static Vector2 GetRoleRandBronPos(ECSWorld world) {
            MapComponent comp = GetMapComponent(world);
            if (comp == null) return Vector2.one;
            return GetRandPos(comp.PlayerBronArea);
        }

        public static Vector2 GetMonsterRandBronPos(ECSWorld world){
            MapComponent comp = GetMapComponent(world);
            if (comp == null) return Vector2.one;
            return GetRandPos(comp.MonsterBronArea[0]);
        }

        public static int GetMapId(ECSWorld world) {
            MapComponent comp = GetMapComponent(world);
            if (comp == null) return -1;
            return comp.MapId;
        }

        public static int GetMapRank(ECSWorld world)
        {
            MapComponent comp = GetMapComponent(world);
            if (comp == null) return -1;
            return comp.RankId;
        }
    }
}
