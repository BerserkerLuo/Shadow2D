using RandMap;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace RandMap
{


    public class RandMapCreator : MonoBehaviour
    {
        public int width;
        public int height;
        public int EdgeWidth = 7;

        [Range(0, 0.2f)]
        public float lacunarity;

        [Range(0, 1000)]
        public int GroundHeight;
        [Range(0, 1000)]
        public int MountainHeight;

        public List<TileMapData> TileMaps;

        public GameObject textRoot;

        public TileBase waterTile;
        public TileBase groundTile;
        public TileBase mountainTile;
        public TileBase wallTile;
        public List<SlopeConfig> slopeTiles;
        public List<ItemSpawnData> itemSpawnDatas;

        public Transform MapRoot;

        public int seed = 0;
        public bool useSeed = false; 

        public bool useSommth = false;
        public bool showHeight = false;
        public void GenerateMap() {

            TryUseSeed();

            float rootx = width / 2 + EdgeWidth;
            float rooty = height / 2 + EdgeWidth;
            MapRoot.position = new Vector3(-rootx,-rooty,0);

            MapMgr.Singleton.InitParams(this);
            MapMgr.Singleton.CreateMap(GetTileMapDict());

            ShowHeightMap(TileMaps[0].tileMap);

        }

        private void TryUseSeed() {
            if (!useSeed) 
                seed = Time.time.GetHashCode();
            UnityEngine.Random.InitState(seed);
        } 
        private Dictionary<TilemapLayer, Tilemap> GetTileMapDict() {
            Dictionary<TilemapLayer, Tilemap> tileMaps = new Dictionary<TilemapLayer, Tilemap>();
            foreach (var it in TileMaps) {
                tileMaps.Add(it.layer,it.tileMap);
                it.tileMap.ClearAllTiles();
            }
            return tileMaps;
        }
         

        void ShowHeightMap(Tilemap tilemap)
        {
            for (; textRoot.transform.childCount > 0;)
                DestroyImmediate(textRoot.transform.GetChild(0).gameObject);

            if (!showHeight)
                return;

            //HeightMap heightMap = MapMgr.Singleton.FertilityMap;
            HeightMap heightMap = MapMgr.Singleton.HeightMap;
            for (int y = 0; y < height + EdgeWidth*2; y++)
                for (int x = 0; x < width + EdgeWidth * 2; x++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    Vector3 worldPos = tilemap.CellToWorld(cellPos);
                    worldPos.x += 2.5f;
                    worldPos.y += 2.5f;

                    GameObject textObj = new GameObject();
                    textObj.transform.parent = textRoot.transform;
                    textObj.transform.position = worldPos;

                    RectTransform rect = textObj.AddComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(5,5);

                    TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
                    text.fontSize = 2;
                    text.alignment = TextAlignmentOptions.Center;
                    text.color = Color.red;

                    text.text = ((int)heightMap.GetMapHeight(y, x)).ToString();
                    //text.text = "" + x + ","+ y;
                    //text.text = heightMap[y, x].ToString("f2");

                }
        }

    }
}