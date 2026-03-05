using System.Collections;
using UnityEngine;

namespace RandMap
{
    public class HeightMap
    {
        public int Height = 0;
        public int Width = 0;
        public int EdgeWidth = 0;

        float Lacunarity = 0;
        float MaxHeight = 1000;
        float MinHeight = 0;
        float[,] mapHeightData;

        public float[,] HeightData { get { return mapHeightData; } }

        public HeightMap(int height,int width,int edgeWidth, float lacunarity,int minHeight = 0,int maxHeight = 1000) {
            Height = height;
            Width = width;
            Lacunarity = lacunarity;
            MinHeight = minHeight;
            MaxHeight = maxHeight;
            EdgeWidth = edgeWidth;
            CreateHeightMap();
        }

        void CreateHeightMap()
        {
            float randomOffset = Random.Range(-10000, 10000);

            float minValue = float.MaxValue;
            float maxValue = float.MinValue;

            mapHeightData = new float[Height, Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * Lacunarity + randomOffset, y * Lacunarity + randomOffset);
                    mapHeightData[y, x] = noiseValue;
                    if (noiseValue < minValue) minValue = noiseValue;
                    if (noiseValue > maxValue) maxValue = noiseValue;
                }
            }

            // 平滑到0~1
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    mapHeightData[y, x] = Mathf.InverseLerp(minValue, maxValue, mapHeightData[y, x]) * (MaxHeight - MinHeight) + minValue;

            //for (int y = 0; y < Height; y++)
            //    for (int x = 0; x < Width; x++)
            //        mapHeightData[y, x] = 0;//mapHeightData[y, x];
        }

        public bool CheckCrossLine(int y, int x){
            if (x < 0 || x >= Width)
                return true;
            if (y < 0 || y >= Height)
                return true;
            return false;
        }
        public float GetMapHeight(int y, int x){
            if (CheckCrossLine(y, x))
                return MinHeight;
            return mapHeightData[y, x];
        }

        public void Setheight(int y,int x,float height) {
            mapHeightData[y, x] = height;
        }

        public int MakePosIndex(int x, int y)
        {
            return y * Width + x;
        }

        public void LerpEage()
        {
            int ty = Height - 1;
            for (int x = 0; x < Width; ++x){
                if (x != 0 && x != Width - 1) {
                    for (int t = 1; t < EdgeWidth; ++t){
                        mapHeightData[t, x] = 0;
                        mapHeightData[ty - t, x] = 0;
                    }
                }
 
                mapHeightData[0, x] = 400;
                mapHeightData[ty, x] = 400;
            }

            int tx = Width - 1;
            for (int y = 0; y < Height; ++y)
            {
                if (y != 0 && y != Height - 1){
                    for (int t = 1; t < EdgeWidth; ++t){
                        mapHeightData[y, t] = 0;
                        mapHeightData[y, tx - t] = 0;
                    }
                }

                mapHeightData[y, 0] = 400;
                mapHeightData[y, tx] = 400;
            }

            for (int x = 0; x < Width; ++x)
            {
                int x1 = x;
                if (x1 < EdgeWidth) x1 += EdgeWidth;
                if (Width - x1 < EdgeWidth) x1 -= EdgeWidth;
                LerpEage(0, x, EdgeWidth, x1);
                LerpEage(ty, x, ty - EdgeWidth, x1);
            }

            for (int y = 0; y < Height; ++y)
            {
                int y1 = y;
                if (y1 < EdgeWidth) y1 += EdgeWidth;
                if (Height - y1 < EdgeWidth) y1 -= EdgeWidth;
                LerpEage(y, 0, y1, EdgeWidth);
                LerpEage(y, tx, y1, tx - EdgeWidth);
            }

            for (int y = 0; y < EdgeWidth; ++y) {
                for (int x = 0; x < EdgeWidth; ++x) {
                    mapHeightData[y, x] = 300;
                    mapHeightData[Height - y-1,Width- x-1] = 300;
                }
            }
        }

        public void LerpEage(int y1, int x1, int y2,int x2) {
            int dist = Mathf.Max(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2));
            int h1 = (int)mapHeightData[y1, x1];
            int h2 = (int)mapHeightData[y2, x2];

            Vector2 startPos = new Vector2(x1, y1);
            Vector2 endPos = new Vector2(x2,y2);
            if (h1 > h2) {
                startPos = new Vector2(x2, y2);
                endPos = new Vector2(x1, y1);
                h1 = (int)mapHeightData[y2, x2];
                h2 = (int)mapHeightData[y1, x1];
            }

            Vector2 prePos = Vector2.zero;
            for (int t = 1; t < dist; ++t) {
                Vector2 tPos = Vector2.Lerp(startPos, endPos, (float)t / dist);
                float h = Mathf.Lerp(h1, h2, (float)t / dist);
                float oldH = mapHeightData[(int)tPos.y, (int)tPos.x];
                mapHeightData[(int)Mathf.Round(tPos.y), (int)Mathf.Round(tPos.x)] = Mathf.Max(h,oldH);
            }
        }
    }
}