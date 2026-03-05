using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RandMap
{
    public class SlopeData
    {
        public Vector2Int dire;
        public List<Vector2Int> SlopePos = new List<Vector2Int>();
    }

    [Serializable]
    public class ItemSpawnData
    {
        public string name;
        public int wegith = 100;
        public int size = 1; //半径为1是1格 
        public bool isBlock = false;
        public bool active = false;
        public List<TileBase> tile;
        [Range(0, 1000)]
        public int minHeight = 0; //高度
        [Range(0, 1000)]
        public int maxHeight = 1000; //高度
        [Range(0, 100)]
        public int minFertility = 0; //肥沃度
        [Range(0, 100)]
        public int maxFertility = 100; //肥沃度

    }

    [Serializable]
    public class SlopeConfig
    {
        public TileBase tile1;
        public TileBase tile2;
        public TileBase tile3;
    }

    [Serializable]
    public class TileMapData
    {
        public TilemapLayer layer;
        public Tilemap tileMap;
    }

}