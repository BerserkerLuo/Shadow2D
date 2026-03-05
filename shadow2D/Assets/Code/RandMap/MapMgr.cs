using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace RandMap
{
    enum LayerType { 
        Ground = 1,
        Mountain = 2,
    }

    public enum TilemapLayer { 
        WaterLayer = 1,
        WallLayer = 2,
        GroundLayer = 3,
        MountainLayer = 4,
        MountainSlopeLayer = 5,
        DecorationLayer = 6,
    }

    public class MapMgr : SingletonBase<MapMgr>
    {
        public int Width;
        public int Height;
        public int EdgeWidth;
        public float lacunarity;
        public int GroundHeight = 0;
        public int MountainHeight = 0;

        public TileBase waterTile;
        public TileBase groundTile;
        public TileBase mountainTile;
        public TileBase wallTile;
        public List<SlopeConfig> slopeTiles;
        public List<ItemSpawnData> itemSpawnDatas;

        HeightMap heightMap;
        public HeightMap HeightMap { get { return heightMap; } }

        HeightMap fertilityMap;
        public HeightMap FertilityMap { get { return fertilityMap; } }

        HashSet<int> wallList = new HashSet<int>();

        Dictionary<TilemapLayer, Tilemap> tileMaps = new Dictionary<TilemapLayer, Tilemap>();
        Dictionary<LayerType, MapLayer> mapLyaer = new Dictionary<LayerType, MapLayer>();

        public bool useSoomth = false;
        public void InitParams(RandMapCreator seting)
        {
            useSoomth = seting.useSommth;

            Width = seting.width + seting.EdgeWidth * 2;
            Height = seting.height + seting.EdgeWidth * 2;
            EdgeWidth = seting.EdgeWidth;
            lacunarity = seting.lacunarity;
            GroundHeight = seting.GroundHeight;
            MountainHeight = seting.MountainHeight;
            waterTile = seting.waterTile;
            groundTile = seting.groundTile;
            mountainTile = seting.mountainTile;
            slopeTiles = seting.slopeTiles;
            itemSpawnDatas = seting.itemSpawnDatas;
            wallTile = seting.wallTile;

            mapLyaer.Clear();
            mapLyaer.Add(LayerType.Ground, CreateMapLayer(GroundHeight,0));
            mapLyaer.Add(LayerType.Mountain, CreateMapLayer(MountainHeight, GroundHeight));
        }

        public MapLayer CreateMapLayer(int layerHeight, int lowLayerHeight)
        {
            MapLayer layer = new MapLayer();
            layer.MapWidth = Width;
            layer.MapHeight = Height;
            layer.tileHeight = layerHeight;
            layer.prevLayerTileHeight = lowLayerHeight;
            return layer;
        }

        public void CreateMap(Dictionary<TilemapLayer, Tilemap> tileMaps) {

            this.tileMaps = tileMaps;

            heightMap = new HeightMap(Height, Width, EdgeWidth, lacunarity);
            fertilityMap = new HeightMap(Height, Width, EdgeWidth, lacunarity*2,0,100);

            heightMap.LerpEage();

            wallList.Clear();

            foreach (var it in mapLyaer)
            {
                MapLayer layer = it.Value;
                layer.RemoveCliff(heightMap);
                layer.SmoothLayer(heightMap);
                layer.RemoveSamllIsLand(heightMap);
            }

            GenerateTileMap(tileMaps.GetValueOrDefault(TilemapLayer.WaterLayer), waterTile, -1, 1001);
            GenerateTileMap(tileMaps.GetValueOrDefault(TilemapLayer.GroundLayer), groundTile, GroundHeight, 1001);
            GenerateTileMap(tileMaps.GetValueOrDefault(TilemapLayer.MountainLayer), mountainTile, MountainHeight, 1001);
    
            //生成隐形墙
            GenerateWall(tileMaps.GetValueOrDefault(TilemapLayer.WallLayer));
            //生成随机斜坡
            GenerateSlope(tileMaps.GetValueOrDefault(TilemapLayer.MountainSlopeLayer), tileMaps.GetValueOrDefault(TilemapLayer.WallLayer));
            //生成地图装饰
            GenerateDecoration(tileMaps.GetValueOrDefault(TilemapLayer.DecorationLayer));
            GenerateDecorationWall(tileMaps.GetValueOrDefault(TilemapLayer.WallLayer));
        }

        //===============================================================================
        //填充地图
        public void GenerateTileMap(Tilemap tilemap,TileBase tile,int heightMin,int heightMax)
        {
            RandMapToolUtil.TimeStartLog("GenerateTileMap Layer :" + tilemap.name);

            for (int x = 0; x < Width; x++){
                for (int y = 0; y < Height; y++){
                    float height = heightMap.GetMapHeight(y, x);
                    if (height < heightMin || height >= heightMax)
                        continue;

                    tilemap.SetTile(new Vector3Int(x, y), tile);
                }
            }

            RandMapToolUtil.TimeEndLog("GenerateTileMap Layer :" + tilemap.name);
        }

        //===============================================================================
        //添加隐形墙
        public void GenerateWall(Tilemap tilemap) {
            foreach (var layerIt in mapLyaer) {
                List<Vector2Int> wall  = layerIt.Value.SearchEage(heightMap);
                foreach (var pos in wall)
                {
                    tilemap.SetTile(new Vector3Int(pos.x, pos.y), wallTile);
                    wallList.Add(heightMap.MakePosIndex(pos.x,pos.y));
                } 
            }
        }

        List<Vector2Int> DecorationWallList = new List<Vector2Int>();
        public void GenerateDecorationWall(Tilemap tilemap){
            foreach (var pos in DecorationWallList){
                tilemap.SetTile(new Vector3Int(pos.x, pos.y), wallTile);
                wallList.Add(heightMap.MakePosIndex(pos.x, pos.y));
            }
        }

        //===============================================================================
        //生成随机斜坡
        public void GenerateSlope(Tilemap slopeTilemap,Tilemap wallTileMap) {
            MapLayer  layer =  mapLyaer.GetValueOrDefault(LayerType.Mountain);
            List<Vector2Int> wall = layer.SearchEage(heightMap);

            List<SlopeData> SlopeDataList = layer.CreateRandmSlope(heightMap, wall);

            foreach (var slopeData in SlopeDataList)
                GenerateSlope(slopeTilemap, slopeData, wallTileMap);
        }

        private void GenerateSlope(Tilemap tilemap, SlopeData slopeData, Tilemap wallTileMap) {
            SlopeConfig slopeConfig = slopeTiles[0];
            if (slopeData.dire.x == -1)
                slopeConfig = slopeTiles[1];
            if (slopeData.dire.y == -1)
                slopeConfig = slopeTiles[2];
            if (slopeData.dire.x == 1)
                slopeConfig = slopeTiles[3];
          

            int lenth = slopeData.SlopePos.Count;
            for (int i = 1; i < lenth - 1; ++i) {
                TileBase tile = slopeConfig.tile2;
                if(i == 1) tile = slopeConfig.tile1;
                if (i == lenth - 2) tile = slopeConfig.tile3;
                Vector2Int pos = slopeData.SlopePos[i];
                tilemap.SetTile(new Vector3Int(pos.x,pos.y), tile);
                if (i != 1 && i != lenth - 2)
                {
                    wallTileMap.SetTile(new Vector3Int(pos.x, pos.y), null);
                    wallList.Remove(heightMap.MakePosIndex(pos.x, pos.y));
                }
            }
        }

        //===============================================================================
        //生成随机装饰

        HashSet<int> DecorationBlock = new HashSet<int>();

        public void GenerateDecoration(Tilemap tileMap){
            DecorationWallList.Clear();
            DecorationBlock.Clear();
            for (int x = 0; x < Width; x++){
                for (int y = 0; y < Height; y++){
                    int index = heightMap.MakePosIndex(x, y);
                    if (wallList.Contains(index))
                        continue;

                    if (DecorationBlock.Contains(index))
                        continue;

                    ItemSpawnData itemData = GetRandTile(x,y);
                    if (itemData == null)
                        continue;

                    TileBase tile = itemData.tile[Random.Range(0, itemData.tile.Count)];

                    tileMap.SetTile(new Vector3Int(x,y),tile);
                   
                    DecorationBlock.Add(index);
                    if (itemData.size > 1){
                        DecorationBlock.Add(index + 1);
                        DecorationBlock.Add(index - 1);
                    }

                    if (itemData.isBlock) {
                        DecorationWallList.Add(new Vector2Int(x,y));
                        if (itemData.size > 1){
                            DecorationWallList.Add(new Vector2Int(x - 1, y));
                            DecorationWallList.Add(new Vector2Int(x + 1, y));
                        }
                    }
                }
            }
        } 

        public ItemSpawnData GetRandTile(int x,int y) {

            if (Random.Range(0, 1000) < 950)
                return null;

            List<int> indexList = new List<int>();
            int totalWeight = 0;
            float height = heightMap.GetMapHeight(y,x);
            float fertility = fertilityMap.GetMapHeight(y,x);
            for (int i = 0;i < itemSpawnDatas.Count;++i) {
                var item = itemSpawnDatas[i];
                if (!item.active)
                    continue;
                if (height < item.minHeight || height >= item.maxHeight)
                    continue;
                if (fertility < item.minFertility || fertility >= item.maxFertility)
                    continue; 
                if (item.size > 1 && CheckWallNeighbour(x, y))
                    continue;
                if (item.isBlock && IsBronArea(x, y))
                    continue;

                totalWeight += item.wegith;
                indexList.Add(i);
            }

            int rand = Random.Range(0, totalWeight);
            foreach (int i in indexList) {
                var item = itemSpawnDatas[i];
                if (item.wegith < rand){
                    rand -= item.wegith;
                    continue;
                }
                return item;
            }

            return null;
        }

        public bool CheckWallNeighbour(int x,int y) {
            int index = heightMap.MakePosIndex(x, y);
            if (DecorationBlock.Contains(index - 1) || DecorationBlock.Contains(index + 1))
                return true;

            foreach (var it in Dire.DireMap){
                if (heightMap.CheckCrossLine(it.Value.y + y, it.Value.x + x))
                    return false;
                int index2 = heightMap.MakePosIndex(it.Value.x + x, it.Value.y + y);
                if (wallList.Contains(index2))
                    return true;
            }

            return false;
        }

        public bool IsBronArea(int x, int y) {
            if (x <= EdgeWidth && y <= EdgeWidth)
                return true;
            if (Width - x <= EdgeWidth && Height - y <= EdgeWidth)
                return true;
            return false;
        }
    }
}