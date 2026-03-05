using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC
{
    public class WFCTile
    {
        public static int AllType = 0;
        static WFCTile() {
            AllType |= 1 << (int)NeighberType.Grass;
            AllType |= 1 << (int)NeighberType.Road;
            AllType |= 1 << (int)NeighberType.Wall;
            AllType |= 1 << (int)NeighberType.Water;
        }

        public TileBase Tile { get; }
        public Dictionary<int, List<int>> DireNeighbour = new Dictionary<int, List<int>>();
        public float Weight { get; }
        public float Weightlog2 { get; }

        public int tileId = 0;
         
        public WFCTile(TileBase tile, TileSeting setting)
        {
            Tile = tile;
            tileId = setting.number;

            setting.InitNeighbour();
            foreach (var it in setting.neighbours) 
                DireNeighbour.Add(it.Key, ToNeighboutList(it.Value));

            Weight = setting.weight;
            Weightlog2 = Mathf.Log(Weight, 2);

            //ToolUtil.Log("TileName[{}] U[{}] R[{}] D[{}] L[{}]", setting.name, setting.Up, setting.Right, setting.Down, setting.Left);
        }

        public List<int> ToNeighboutList(List<TileSeting> list) {
            List<int> retList = new List<int>();
            foreach (TileSeting it in list)
                retList.Add(it.number);
            return retList;
        }
    }
}