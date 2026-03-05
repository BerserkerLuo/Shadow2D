using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WFC
{


    public class OutputGrid
    {
        Dictionary<int, HashSet<int>> indexPossiblePatternsDict = new Dictionary<int, HashSet<int>>();

        public int width { get; }
        public int height { get; }
         
        public OutputGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            ResetAllPossibilities();
        }

        public void ResetAllPossibilities()
        {
            List<int> tileIds = TileManager.Singleton.GetTileIdList();
            indexPossiblePatternsDict.Clear();
            for (int i = 0; i < width * height; i++){
                indexPossiblePatternsDict.Add(i, new HashSet<int>(tileIds));
            }
        }

        public bool CheckCellExists(Vector2Int position)
        {
            int index = GetIndexFromCoords(position.x, position.y);
            return indexPossiblePatternsDict.ContainsKey(index);

        }

        public bool CheckIfCellIsCollapsed(Vector2Int position)
        {
            return GetPossibleValuesForPositon(position).Count <= 1;
        }

        public bool CheckIfGridIsSolved()
        {
            return !indexPossiblePatternsDict.Any(x => x.Value.Count > 1);
        }

        internal bool CheckIfValidPosition(Vector2Int position)
        {
            return ToolUtil.ValidateCoordinates(position.x, position.y, this.width, this.height);
        }

        public HashSet<int> GetPossibleValuesForPositon(Vector2Int position)
        {
            return GetPossibleValuesForPositon(position.x,position.y);
        }

        public HashSet<int> GetPossibleValuesForPositon(int x,int y)
        {
            int index = GetIndexFromCoords(x, y);
            if (indexPossiblePatternsDict.ContainsKey(index))
                return indexPossiblePatternsDict[index];
            return new HashSet<int>();
        }

        public Vector2Int GetRandomCell()
        {
            int randomIndex = UnityEngine.Random.Range(0, indexPossiblePatternsDict.Count);
            return GetCoordsFromIndex(randomIndex);
        }

        public Vector2Int GetCoordsFromIndex(int index)
        {
            Vector2Int coordsVector = Vector2Int.zero;
            coordsVector.x = index / this.width;
            coordsVector.y = index % this.height;
            return coordsVector;
        }

        public void SetPatternOnPosition(int x, int y, int patternIndex)
        {
            int index = GetIndexFromCoords(x, y);
            indexPossiblePatternsDict[index] = new HashSet<int>() { patternIndex };

        }

        private int GetIndexFromCoords(int x, int y)
        {
            return x + width * y;
        }

        public void PrintResultsToConsol()
        {
            List<String> textToPrint = new List<string>();
            StringBuilder b;
            for (int i = 0; i < this.height; i++)
            {
                b = new StringBuilder();
                for (int j = 0; j < this.width; j++)
                {
                    var result = GetPossibleValuesForPositon(new Vector2Int(j, i));
                    if (result.Count == 1)
                        b.Append(result.First() + "   ");
                    else
                    {
                        //string newString = "C" + result.Count;
                        string newString = "";
                        foreach (var item in result)
                        {
                            newString += item + ",";
                        }
                        b.Append(newString + " ");
                    }
                }
                textToPrint.Add(b.ToString());
            }
            textToPrint.Reverse();
            foreach (var item in textToPrint)
            {
                Debug.Log(item);
            }
            Debug.Log("---");
        }

        public int[][] GetSolvedOutputGrid()
        {
            int[][] returnGrid = ToolUtil.CreateJaggedArray<int[][]>(this.height, this.width);
            //if (CheckIfGridIsSolved() == false)
            //{
            //    return MyCollectionExtension.CreateJaggedArray<int[][]>(0, 0);
            //}
            for (int i = 0; i < returnGrid.Length; i++)
            {
                for (int j = 0; j < returnGrid[0].Length; j++)
                {
                    int index = GetIndexFromCoords(j, i);
                    if (indexPossiblePatternsDict[index].Count != 1)
                        returnGrid[i][j] = -1;
                    else
                        returnGrid[i][j] = indexPossiblePatternsDict[index].First();
                }
            }
            return returnGrid;
        }

        public Dictionary<int, HashSet<int>> GetPossibleValues() {
            return indexPossiblePatternsDict;
        }
    }

}
