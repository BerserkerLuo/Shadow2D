using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC
{
    public class ToolUtil
    {
        public static void CreateOutput(int[][] outputValues, int width, int height,Tilemap outputImage)
        {
            if (outputValues.Length == 0)
                return;

            outputImage.ClearAllTiles();

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int tileId = outputValues[row][col];
                    if (tileId == -1) continue;

                    TileBase tile = TileManager.Singleton.GetTileBase(tileId);
                    outputImage.SetTile(new Vector3Int(col, row, 0), tile);
                }
            }
        }

        private static int GetIndexFromCoords(int x, int y, int width)
        {
            return x + width * y;
        }
        public static void CreateOutput(Dictionary<int, HashSet<int>> outputValues, int width, int height, Tilemap outputImage)
        {
            int tileCount = TileManager.Singleton.GetTileNum();
            int CellWidth = (int)Mathf.Sqrt(tileCount) + 1;
            
            outputImage.ClearAllTiles();

            for (int y = 0; y < height; y++){
                for (int x = 0; x < width; x++){
                    int index = GetIndexFromCoords(x, y, width);
                    HashSet<int> IdList = outputValues.GetValueOrDefault(index, null);
                    CreateOutput(IdList,x* (CellWidth+2) + 1, y* (CellWidth+2 )+ 1, CellWidth, outputImage);
                }
            }
        }

        static void CreateOutput(HashSet<int> outputValues, int x,int y,int CellWidth, Tilemap outputImage) {
            int width = (int)Mathf.Sqrt(outputValues.Count) + 1;
            int index = 0;
            foreach (int tileId in outputValues) {

                int stepX = index % CellWidth;
                int stepy = index / CellWidth;

                TileBase tile = TileManager.Singleton.GetTileBase(tileId);
                outputImage.SetTile(new Vector3Int(x + stepX, y + stepy, 0), tile);

                index++;
            }
        }

        public static T CreateJaggedArray<T>(params int[] lengths)
        {
            return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }

        static object InitializeJaggedArray(Type type, int index, int[] lengths)
        {
            Array array = Array.CreateInstance(type, lengths[index]);
            Type elementType = type.GetElementType();

            if (elementType != null)
            {
                for (int i = 0; i < lengths[index]; i++)
                {
                    array.SetValue(
                        InitializeJaggedArray(elementType, index + 1, lengths), i);
                }
            }

            return array;
        }

        public static bool ValidateCoordinates(int x, int y, int width, int height)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return false;
            return true;
        }

        public static string merageLogStr(params object[] paramsList)
        {
            if (paramsList.Length < 1)
                return "";

            string str = paramsList[0].ToString();
            for (int i = 1; i < paramsList.Length; ++i)
            {
                int index = str.IndexOf("{}");
                if (index != -1)
                    str = str.Remove(index, 2).Insert(index, paramsList[i].ToString());
                else
                    str += paramsList[i].ToString();
            }
            return str;
        }


        public static void Log(params object[] paramsList)
        {
            Debug.Log(merageLogStr(paramsList));
        }
    }
}