using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    public class VectorPair
    {
        private Vector2Int PrevPos { get; set; } //上一步格子
        public Vector2Int BasePos { get; set; } //正在传播的格子
        public Vector2Int Pos { get; set; } //接受传播的格子
        
        public int Dire { get; set; }

        public VectorPair(Vector2Int baseCellPosition, Vector2Int cellToPropagatePosition, int directionFromBase, Vector2Int previousCellPosition)
        {
            this.BasePos = baseCellPosition;
            this.Pos = cellToPropagatePosition;
            this.Dire = directionFromBase;
            this.PrevPos = previousCellPosition;
        }

        public bool AreWeCheckingPreviousCellAgain()
        {
            return PrevPos == Pos;
        }
    }
    
}

    


