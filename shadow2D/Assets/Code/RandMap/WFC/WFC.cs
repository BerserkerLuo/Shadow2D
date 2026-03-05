using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC
{
    public class WFC2
    {
        List<Tilemap> inputTiles;
        Tilemap outTileMap;
        int outputWidth;
        int outputHeight;

        public WFC2(List<Tilemap> input, Tilemap outTile,int Width,int Height) {
            inputTiles = input;
            outTileMap = outTile;
            outputWidth = Width;
            outputHeight = Height;

            TileManager.Singleton.Init(inputTiles);
            EntropyMgr.Singleton.Init();
            Random.InitState(1);
        }

        WFCCore2 core;
        public void CreateNewTileMap() {
            core = new WFCCore2(this.outputWidth,this.outputHeight);
            int[][] grid = core.ExcuteTick();
            ToolUtil.CreateOutput(grid, outputWidth, outputHeight, outTileMap);
        }

        public void TestCreate() {
            core = new WFCCore2(this.outputWidth, this.outputHeight);
        }

        public void ExcuteTick() {
            int[][] grid = core.ExcuteTick();

            //ToolUtil.CreateOutput(grid, outputWidth, outputHeight, outTileMap);

            Dictionary<int, HashSet<int>> PossibleValues = core.GetPossibleValues();
            ToolUtil.CreateOutput(PossibleValues, outputWidth, outputHeight, outTileMap);
        }

    }
}