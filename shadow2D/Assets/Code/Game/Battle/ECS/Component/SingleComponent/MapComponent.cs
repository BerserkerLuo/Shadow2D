
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class Area {
        public Vector2 startPos;
        public Vector2 endPos;
        public Vector2 length;
    }

    public class MapComponent : Component
    {
        public int MapId = 0;
        public int RankId = 0;

        public int MapWidth = 0;
        public int MapHeight = 0;
        public int EdgeWidth = 0;

        public Area PlayerBronArea = new();
        public List<Area> MonsterBronArea = new();
    }
}