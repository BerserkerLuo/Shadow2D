using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WFC
{
    public class CoreHelper 
    {
        public static float totalFrequenc = 0;
        public static float totalFrequencyLog = 0;

        public static float CalculateEntropy(Vector2Int position, OutputGrid outputGrid)
        {
            float sum = 0;
            foreach (var possibleIndex in outputGrid.GetPossibleValuesForPositon(position))
                sum += TileManager.Singleton.GetTileWeightLog2(possibleIndex);

            return totalFrequencyLog - (sum / totalFrequenc);
        }
        public static int SelectRandomTileIndex(List<int> possibleValues)
        {
            List<float> valueFrequenciesFractions = GetListOfWeightsFromIndices(possibleValues);
            float randomValue = Random.Range(0, valueFrequenciesFractions.Sum());
            float sum = 0;
            int index = 0;
            foreach (var item in valueFrequenciesFractions)
            {
                sum += item;
                if (randomValue <= sum)
                    return index;
                index++;
            }
            return index;
        }

        private static List<float> GetListOfWeightsFromIndices(List<int> possibleValues)
        {
            var valueFrequencies = possibleValues.Aggregate(new List<float>(), (acc, val) =>{
                acc.Add(TileManager.Singleton.GetTileWeight(val));
                return acc;
            },
            acc => acc).ToList();
            return valueFrequencies;
        }

        //判断传入的位置是否和周围冲突
        public static bool CheckCellSOlutionForCollisions(Vector2Int Pos,OutputGrid outputGrid) {

            HashSet<int> valuesList = outputGrid.GetPossibleValuesForPositon(Pos);

            foreach (VectorPair neighbour in Create4DirectionNeighbours(Pos))
            {
                //越界了
                if (outputGrid.CheckIfValidPosition(neighbour.Pos) == false)
                    continue;

                int dire = Dire.GetReverseDire(neighbour.Dire);
                HashSet<int> tempList = new HashSet<int>();
                HashSet<int> direValuesList = outputGrid.GetPossibleValuesForPositon(neighbour.Pos);
                foreach (int tileId in direValuesList) {
                    WFCTile tile = TileManager.Singleton.GetTile(tileId);
                    if (tile == null)
                        continue;
                    List<int> list = tile.DireNeighbour.GetValueOrDefault(dire, null);
                    if (list == null)
                        continue;
                    tempList.UnionWith(list);
                }

                if (tempList.Contains(valuesList.First()) == false) {
                    return true;
                }
            }

            return false;
        }

        public static List<VectorPair> Create4DirectionNeighbours(Vector2Int Pos, Vector2Int previousCell)
        {
            List<VectorPair> list = new List<VectorPair>()
            {
                new VectorPair(Pos,new Vector2Int(Pos.x, Pos.y+1), Dire.Up, previousCell),
                new VectorPair(Pos,new Vector2Int(Pos.x+1, Pos.y), Dire.Right,previousCell),
                new VectorPair(Pos,new Vector2Int(Pos.x, Pos.y-1), Dire.Down, previousCell),
                new VectorPair(Pos,new Vector2Int(Pos.x-1, Pos.y), Dire.Left, previousCell)
            };
            return list;
        }

        public static List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinates)
        {
            return Create4DirectionNeighbours(cellCoordinates, cellCoordinates);
        }

    }
}