using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WFC
{
   

    public class Solver
    {
        OutputGrid outputGrid;

        PropagationHelper propagationHelper;
        CoreHelper coreHelper;

       

        public Solver(OutputGrid outputGrid) {
            this.outputGrid = outputGrid;
            coreHelper = new CoreHelper();
            propagationHelper = new PropagationHelper();
        }
        //=============================================================================
        //坍缩最小熵值单元
        public void CollapseLowestEntropyCell()
        {
            Vector2Int pos = GetLowestEntropyCell();
            CollapseCell(pos);
        }

        //获取最小熵值单元
        public Vector2Int GetLowestEntropyCell()
        {
            if (EntropyMgr.Singleton.lowestEntropySet.Count <= 0)
                return outputGrid.GetRandomCell();

            return EntropyMgr.Singleton.PopCell();
        }

        //坍缩指定单元格
        public void CollapseCell(Vector2Int Pos)
        {
            var possibleValues = outputGrid.GetPossibleValuesForPositon(Pos).ToList();

            if (possibleValues.Count == 0 || possibleValues.Count == 1)
                return;
            else
            {
                int index = CoreHelper.SelectRandomTileIndex(possibleValues);
                outputGrid.SetPatternOnPosition(Pos.x, Pos.y, possibleValues[index]);
            }

            if (CoreHelper.CheckCellSOlutionForCollisions(Pos, outputGrid) == false)
                propagationHelper.AddNewPairsToPropagateQueue(Pos, Pos);
            else
                propagationHelper.SetConflictFlag();
        }

        //传播约束
        public void Propagate() {

            while (propagationHelper.PairsToPropagate.Count > 0)
            {
                var propagatePair = propagationHelper.PairsToPropagate.Dequeue();
                if (CheckIfPairShouldBeProcessed(propagatePair))
                {
                    ProcessCells(propagatePair);
                }
                if (propagationHelper.CheckForConflics() || outputGrid.CheckIfGridIsSolved())
                {
                    return;
                }
            }

            if (propagationHelper.CheckForConflics() && propagationHelper.PairsToPropagate.Count == 0 && EntropyMgr.Singleton.lowestEntropySet.Count == 0)
            {
                propagationHelper.SetConflictFlag();
            }
        }

        //检查单元格是否需要传播约束
        public bool CheckIfPairShouldBeProcessed(VectorPair propagatePair)
        {
            return outputGrid.CheckIfValidPosition(propagatePair.Pos) && propagatePair.AreWeCheckingPreviousCellAgain() == false;
        }

        public void ProcessCells(VectorPair propagatePair) {
            if (outputGrid.CheckIfCellIsCollapsed(propagatePair.Pos))
            {
                propagationHelper.EnqueueUncollapsedNeighbours(propagatePair,outputGrid);
            }
            else
            {
                PropagateNeighbours(propagatePair);
            }
        }

        public void PropagateNeighbours(VectorPair propagatePair) {
            var possibleValuesAtNeighbour = outputGrid.GetPossibleValuesForPositon(propagatePair.Pos);
            int startCount = possibleValuesAtNeighbour.Count();

            RemoverImpossibleNeighbours(propagatePair, possibleValuesAtNeighbour);

            int newPossiblePatternCount = possibleValuesAtNeighbour.Count;

            propagationHelper.AnalyzePropagatonResults(propagatePair, startCount, newPossiblePatternCount,outputGrid);
        }

        private void RemoverImpossibleNeighbours(VectorPair propagatePair, HashSet<int> possibleValues)
        {
            HashSet<int> possibleIndices = new HashSet<int>();

            HashSet<int> baseValues = outputGrid.GetPossibleValuesForPositon(propagatePair.BasePos);
            foreach (int tileId in baseValues) {
                WFCTile tile = TileManager.Singleton.GetTile(tileId);
                if (tile == null)
                    continue;
                List<int> list = tile.DireNeighbour.GetValueOrDefault(propagatePair.Dire, null);
                if (list == null)
                    continue;

                possibleIndices.UnionWith(list);
            }

            string str = "";
            foreach (int i in possibleIndices) str += i + ",";
            ToolUtil.Log("RemoverImpossibleNeighbours Pos{} possibleIndices{}", propagatePair.Pos.ToString(), str);

            possibleValues.IntersectWith(possibleIndices);
        }

        //=============================================================================
        //是否生成完毕
        public bool CheckIfSolved()
        {
            return outputGrid.CheckIfGridIsSolved();
        }

        //是否有无法解决的冲突
        public bool CheckForConflics()
        {
            return false;
        }
    }
}