using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WFC
{
    public class PropagationHelper 
    {
        SortedSet<LowEntropyCell> lowestEntropySet = new SortedSet<LowEntropyCell>();

        Queue<VectorPair> pairsToPropagate = new Queue<VectorPair>();
        public Queue<VectorPair> PairsToPropagate { get { return pairsToPropagate; } }

        public bool ConflictFlag = false;

        public void AddNewPairsToPropagateQueue(Vector2Int cellCoordinates, Vector2Int previousCell)
        {
            var list = CoreHelper.Create4DirectionNeighbours(cellCoordinates, previousCell);
            foreach (var item in list)
                pairsToPropagate.Enqueue(item);
        }

        public void EnqueueUncollapsedNeighbours(VectorPair propagatePair, OutputGrid outputGrid) {
            var uncollapsedNeighbours = CheckIfNeighboursAreCollapsed(propagatePair, outputGrid);
            foreach (var uncollapsed in uncollapsedNeighbours)
                pairsToPropagate.Enqueue(uncollapsed);
        }

        public List<VectorPair> CheckIfNeighboursAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid)
        {
            return CoreHelper.Create4DirectionNeighbours(pairToCheck.Pos, pairToCheck.BasePos)
                .Where(x => outputGrid.CheckIfValidPosition(x.Pos) && outputGrid.CheckIfCellIsCollapsed(x.Pos) == false)
                .ToList();
        }


        public void AnalyzePropagatonResults(VectorPair propagatePair, int startCount, int newPossiblePatternCount,OutputGrid outputGrid)
        {
            if (newPossiblePatternCount == 0)
            {
                ConflictFlag = true;
                return;
            }

            if (startCount > newPossiblePatternCount)
            {
                AddNewPairsToPropagateQueue(propagatePair.Pos, propagatePair.BasePos);
                EntropyMgr.Singleton.AddToLowestEntropySet(propagatePair.Pos,outputGrid);
            }
           
            if (newPossiblePatternCount == 1)
            {
                ConflictFlag = CoreHelper.CheckCellSOlutionForCollisions(propagatePair.Pos, outputGrid);
            }
        }



        public bool CheckForConflics() {
            return ConflictFlag;
        }

        public void SetConflictFlag() {
            ConflictFlag = true;
        }
    }
}