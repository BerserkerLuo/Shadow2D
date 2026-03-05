using System.Collections;
using UnityEngine;

namespace WFC
{
    
    public class Dire 
    {
        public const int Up = 0;
        public const int Right = 1;
        public const int Down = 2;
        public const int Left = 3;

        public static int GetReverseDire(int dire) {
            return (dire + 2) % 4;
        }
    }
}