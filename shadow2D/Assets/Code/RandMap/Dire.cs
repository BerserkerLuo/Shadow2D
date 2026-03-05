using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandMap
{
    public class Dire
    {
        public static Dictionary<int, Vector2Int> DireMap = new Dictionary<int, Vector2Int>();

        static Dire() {
            DireMap.Add(0, Vector2Int.up);      //上
            DireMap.Add(1, Vector2Int.left);    //左
            DireMap.Add(2, Vector2Int.down);    //下
            DireMap.Add(3, Vector2Int.right);   //右
            DireMap.Add(4, new Vector2Int(-1,1)); //左上
            DireMap.Add(5, new Vector2Int(-1, -1)); //左下
            DireMap.Add(6, new Vector2Int(1, -1)); //右下
            DireMap.Add(7, new Vector2Int(1, 1)); //右上
        }
    }
}