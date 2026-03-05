using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RandMap
{
    public class MapLayer
    {
        public TileBase Tile;
        public Tilemap tileMap;
        public int tileHeight = 0;
        public int prevLayerTileHeight = 0;
        public int MapWidth = 0;
        public int MapHeight = 0;

        //======================================================================
        public bool IsThisLayer(HeightMap heightMap, int x, int y)
        {
            return heightMap.GetMapHeight( y, x) >= tileHeight;
        }

        public bool IsPrevLayer(HeightMap heightMap, int x, int y)
        {
            return heightMap.GetMapHeight( y, x) < tileHeight;
        }

        //======================================================================
        //平滑边缘

        public void SmoothLayer(HeightMap heightMap) {
            RandMapToolUtil.TimeStartLog("SmoothLayer");
            for (var i = 0; i < 5; i++)
                if (smoothLayer2(heightMap) == false)
                    break;
            RandMapToolUtil.TimeEndLog("SmoothLayer");
        }

        private bool smoothLayer2(HeightMap heightMap) {
            bool res = false;
            for (int y = 0; y < MapHeight; y++) {
                for (int x = 0; x < MapWidth; x++)
                {
                    if (IsThisLayer(heightMap, x, y) == false)
                        continue;

                    if (!checkCanSmoothByNeighbour(heightMap, x, y, tileHeight))
                        continue;

                    heightMap.Setheight(y, x, tileHeight-1);
                    res = true;
                }
            }
            return res;
        }

        private bool checkCanSmoothByNeighbour(HeightMap heightMap, int x, int y, int tileHeight) {

            List<Vector2Int> direList = getFourNeighborsDire(heightMap, x, y, tileHeight, 0, 4);
            //三个直线次级邻居 需要平滑
            if (direList.Count > 2)
                return true;
            //2个直线次级令居,但是处于对焦 需要平滑
            if (direList.Count == 2 && CalcCheckFlag(direList[0], direList[1]) == 0)
                return true;

            List<Vector2Int> direList2 = getFourNeighborsDire(heightMap, x, y, tileHeight, 4, 8);
            //2个次级令居,但是处于对角 需要平滑
            if (direList2.Count == 2 && CalcCheckFlag(direList2[0], direList2[1]) == 0)
                return true;

            if (direList.Count == 1 && direList2.Count == 1 && CalcCheckFlag(direList[0], direList2[0]) == 1)
                return true;

            return false;
        }

        private int CalcCheckFlag(Vector2Int v1,Vector2Int v2) {
            Vector2Int t = v1 + v2;
            return Mathf.Abs(t.x) + Mathf.Abs(t.y);
        }

        //获取周围次级层的邻居的各自方向
        private List<Vector2Int> getFourNeighborsDire(HeightMap heightMap, int x, int y, int value, int direStart, int direEnd)
        {
            List<Vector2Int>  tempList = new List<Vector2Int>();
            for (int dire = direStart; dire < direEnd; ++dire) {
                Vector2Int direPos = Dire.DireMap.GetValueOrDefault(dire, Vector2Int.zero);
                if (direPos == Vector2Int.zero)
                    continue;
                if (heightMap.GetMapHeight(direPos.y + y, direPos.x + x) >= value)
                    continue;
                tempList.Add(direPos);
            }
            return tempList;
        }

        //======================================================================
        //移除陡峭悬崖
        public void RemoveCliff(HeightMap heightMap)
        {
            RandMapToolUtil.TimeStartLog("RemoveCliff");

            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++)
                {
                    if (!IsThisLayer(heightMap, x, y))
                        continue;

                    if (!HaveLowerHeightNeighbour(heightMap, y, x,prevLayerTileHeight))
                        continue;
                    //周围存在更低层级的地块就降级
                    heightMap.Setheight(y, x, tileHeight - 1);
                }
            RandMapToolUtil.TimeEndLog("RemoveCliff");
        }

        private bool HaveLowerHeightNeighbour(HeightMap heightMap, int y, int x,int height)
        {
            for (int dire = 0; dire < 8; ++dire)
            {
                Vector2Int direPos = Dire.DireMap.GetValueOrDefault(dire, Vector2Int.zero);
                if (direPos == Vector2Int.zero)
                    continue;

                if (heightMap.GetMapHeight(direPos.y + y, direPos.x + x) >= height)
                    continue;

                return true;
            }
            return false;
        }

        //======================================================================
        //移除较小孤岛
        HashSet<int> CloseList = new HashSet<int>();
        public void RemoveSamllIsLand(HeightMap heightMap) {
            RandMapToolUtil.TimeStartLog("RemoveSamllIsLand");
            CloseList.Clear();
            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++) {
                    if (!IsThisLayer(heightMap, x, y))
                        continue;
                    if (inCloseList(x, y))
                        continue;
                    removeSamllIsLand(heightMap, x, y);
                }
            RandMapToolUtil.TimeStartLog("RemoveSamllIsLand");
        }

        private void removeSamllIsLand(HeightMap heightMap, int x, int y) {
            List<Vector2Int> blockList = findTileCount(heightMap, x, y);
            //Debug.Log("blockList size:"+ blockList.Count);
            if (blockList.Count > 16)
                return;

            foreach (var it in blockList)
                heightMap.Setheight(it.y, it.x, tileHeight - 1);
        }

        private int makePosIndex(int x, int y) {
            return y * MapWidth + x;
        }

        private bool inCloseList(int x, int y) {
            return CloseList.Contains(makePosIndex(x, y));
        }

        Queue<Vector2Int> FindQue = new Queue<Vector2Int>();
        private List<Vector2Int> findTileCount(HeightMap heightMap, int x, int y) {
            List<Vector2Int> retList = new List<Vector2Int>();

            retList.Add(new Vector2Int(x, y));
            FindQue.Enqueue(new Vector2Int(x, y));
            CloseList.Add(makePosIndex(x, y));

            for (int i = 0; i < 10000 && FindQue.Count > 0; ++i) {
                Vector2Int pos = FindQue.Dequeue();
                for (int dire = 0; dire < 8; ++dire) {
                    Vector2Int direVec = Dire.DireMap.GetValueOrDefault(dire, Vector2Int.zero);
                    if (direVec == Vector2Int.zero)
                        continue;

                    Vector2Int direPos = pos + direVec;
                    if (!IsThisLayer(heightMap, direPos.x, direPos.y))
                        continue;

                    if (inCloseList(direPos.x, direPos.y))
                        continue;
                    CloseList.Add(makePosIndex(direPos.x, direPos.y));

                    retList.Add(direPos);
                    FindQue.Enqueue(direPos);
                }
            }
            return retList;
        }

        //======================================================================
        //查找本层边缘
        public List<Vector2Int> SearchEage(HeightMap heightMap)
        {
            List<Vector2Int> retList = new List<Vector2Int>();
            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++)
                {
                    if (!IsThisLayer(heightMap, x, y))
                        continue;

                    if (!HaveLowerHeightNeighbour(heightMap, y, x,tileHeight))
                        continue;

                    Vector2Int pos = new Vector2Int(x, y);
                    retList.Add(pos);
                }

            return retList;
        }

        //======================================================================
        //在本层生成随机门

        HashSet<int> createIndexList = new HashSet<int>();
        HashSet<int> walIndexList = new HashSet<int>();
        List<List<Vector2Int>> wallSet = new List<List<Vector2Int>>();
        public List<SlopeData> CreateRandmSlope(HeightMap heightMap, List<Vector2Int> eageList) {
            List<SlopeData> retList = new List<SlopeData>();
            if (eageList.Count == 0)
                return retList;

            createIndexList.Clear();

            walIndexList.Clear();
            foreach (var it in eageList)
                walIndexList.Add(makePosIndex(it.x,it.y));

            for (int i = 0; i < 100; ++i) {
                int randIndex = Random.Range(0, eageList.Count);
                //Debug.Log("eageList.Count " + eageList.Count + " randIndex " + randIndex);
                Vector2Int pos = eageList[randIndex];
                if (pos.x == 1 || pos.x == MapWidth - 2)
                    continue;
                if (pos.y == 1 || pos.y == MapHeight - 2)
                    continue;

                SlopeData slope = tryCreateSlope(heightMap, pos);
                if (slope == null)
                    continue;
                retList.Add(slope);
                foreach (var it in slope.SlopePos)
                    createIndexList.Add(makePosIndex(it.x,it.y));
            }

            return retList;
        }

        //寻找长度>5的墙
        public SlopeData tryCreateSlope(HeightMap heightMap, Vector2Int pos) {
            List<Vector2Int> wallPos = new List<Vector2Int>();
            Vector2Int dire = new Vector2Int(0, 1);
            for (int i = 0; i < 2; i++) {
                searchWall(heightMap, wallPos, pos, dire);
                if (wallPos.Count < 5){
                    int t = dire.x;
                    dire.x = -dire .y;
                    dire.y = t;
                    wallPos.Clear();
                    continue;
                }

                SlopeData slope = new SlopeData();
                slope.SlopePos = wallPos;
                slope.dire = CalcSlopeDire(heightMap, wallPos, dire);
                return slope;
            }
            return null;
        }

        public Vector2Int CalcSlopeDire(HeightMap heightMap, List<Vector2Int> hWall, Vector2Int dire) {
            int midIndex = hWall.Count / 2 + 1;
            Vector2Int midPos = hWall[midIndex];
            Vector2Int checkDire = new Vector2Int(-dire.y,dire.x);
            if (!IsThisLayer(heightMap, midPos.x + checkDire.x, midPos.y + checkDire.y))
                return checkDire;
            else 
                return Vector2Int.zero - checkDire;
        } 
         
        private void searchWall(HeightMap heightMap, List<Vector2Int> wallList, Vector2Int pos,Vector2Int dire) {
            int start = 0;
            for (int loop = 0; loop < 2; ++loop) {
                for (int i = start; i < 10; ++i){
                    Vector2Int newPos = pos + dire*i;
                    int index = makePosIndex(newPos.x,newPos.y);
                    if (!walIndexList.Contains(index))
                        break;

                    if (createIndexList.Contains(index))
                        break;

                    if (IsThisLayer(heightMap,newPos.x - dire.y, newPos.y + dire.x) &&
                        IsThisLayer(heightMap, newPos.x - dire.y, newPos.y + dire.x))
                        break;

                    if (heightMap.GetMapHeight(newPos.y + dire.x*2, newPos.x - dire.y * 2) < prevLayerTileHeight ||
                       heightMap.GetMapHeight(newPos.y + dire.x*2, newPos.x - dire.y * 2) < prevLayerTileHeight)
                        break;

                    wallList.Add(newPos);
                }

                if (loop == 1)
                    break;

                dire = Vector2Int.zero - dire;
                wallList.Reverse();
                start = 1;
            }
        }
    }
} 