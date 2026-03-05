using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC
{
    public class TileManager : SingletonBase<TileManager>
    {
        int MakeIndex = 0;
        Dictionary<int, WFCTile> tiles = new Dictionary<int, WFCTile>();

        //类型 << 2 | dire
        Dictionary<int, HashSet<int>> neighbours = new Dictionary<int, HashSet<int>>();

        public void Init(List<Tilemap> inputs) {

            tiles.Clear();
            MakeIndex = 0;

            InitWFCTile(inputs);
            //InitNeighbours();
        }

        //======================================================================

        public void InitWFCTile(List<Tilemap> inputs) {

            int count = inputs.Count;
            float totalWeight = 0;
            for (int i = 0; i < count; i++)
            {
                var map = inputs[i];
                var tile = map.GetTile(new Vector3Int(0, 0, 0));
                var setting = map.gameObject.GetComponent<TileSeting>();
                tiles.Add(setting.number, new WFCTile(tile, setting));
                totalWeight += setting.weight;
            }

            CoreHelper.totalFrequenc = totalWeight;
            CoreHelper.totalFrequencyLog = Mathf.Log(totalWeight, 2);
        }

        public WFCTile GetTile(int key) {
            return tiles.GetValueOrDefault(key, null);
        }

        public TileBase GetTileBase(int key) {
            WFCTile wfcTile = tiles.GetValueOrDefault(key, null);
            if (wfcTile == null)
                return null;
            return wfcTile.Tile;
        }

        public int GetTileNum() {
            return tiles.Count;
        }

        public List<int> GetTileIdList() {
            List<int> retList = new List<int>();
            retList.AddRange(tiles.Keys);
            return retList;
        }

        public float GetTileWeight(int key)
        {
            WFCTile wfcTile = tiles.GetValueOrDefault(key, null);
            if (wfcTile == null)
                return 0;
            return wfcTile.Weight;
        }

        public float GetTileWeightLog2(int key)
        {
            WFCTile wfcTile = tiles.GetValueOrDefault(key, null);
            if (wfcTile == null)
                return 0;
            return wfcTile.Weightlog2;
        }
        //======================================================================
        //public void InitNeighbours()
        //{
        //    HashSet<int> temp = null;
        //    foreach (var it in tiles)
        //    {

        //        foreach (var it2 in it.Value.DireFlag)
        //        {
        //            int flag = MakeNeighbourFlag(Dire.GetReverseDire(it2.Key), it2.Value);
        //            if (neighbours.TryGetValue(flag, out temp) == false)
        //            {
        //                temp = new HashSet<int>();
        //                neighbours.Add(flag, temp);
        //            }
        //            temp.Add(it.Key);
        //        }
        //    }
        //}

        //public int MakeNeighbourFlag(int Dire, int TileType)
        //{
        //    return TileType << 2 | Dire;
        //}

        //HashSet<int> Default = new HashSet<int>();
        //public HashSet<int> GetNeighboursByDire(int die,int typeType) {
        //    int flag = MakeNeighbourFlag(die,typeType);
        //    return neighbours.GetValueOrDefault(flag,Default);
        //}

        //public HashSet<int> GetNeighbours(int key,int dire) {
        //    WFCTile wfcTile = tiles.GetValueOrDefault(key, null);
        //    if (wfcTile == null)
        //        return Default;
        //    int flag = wfcTile.DireFlag.GetValueOrDefault(dire,0);
        //    return GetNeighboursByDire(dire, flag);
        //}
    }
}