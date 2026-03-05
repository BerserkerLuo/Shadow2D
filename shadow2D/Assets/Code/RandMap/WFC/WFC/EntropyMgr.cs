using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WFC
{
    public class EntropyMgr : SingletonBase<EntropyMgr>
    {
        public SortedSet<LowEntropyCell> lowestEntropySet = new SortedSet<LowEntropyCell>();


        public void Init() {
            lowestEntropySet.Clear();
        }

        public void AddToLowestEntropySet(Vector2Int cellToPropagatePosition, OutputGrid outputGrid)
        {
            var elementIdLowEntropySet = lowestEntropySet.Where(x => x.Position == cellToPropagatePosition).FirstOrDefault();
            if (elementIdLowEntropySet != null)
            {
                lowestEntropySet.Remove(elementIdLowEntropySet);
                elementIdLowEntropySet.Entropy = CoreHelper.CalculateEntropy(cellToPropagatePosition, outputGrid);
                lowestEntropySet.Add(elementIdLowEntropySet);
            }

            if (outputGrid.CheckIfCellIsCollapsed(cellToPropagatePosition) == false)
            {
                float entropy = CoreHelper.CalculateEntropy(cellToPropagatePosition, outputGrid);
                lowestEntropySet.Add(new LowEntropyCell(cellToPropagatePosition, entropy));
            }
        }

        public Vector2Int PopCell() {
            var lowestEntropyElement = lowestEntropySet.First();
            Vector2Int returnVEctor = lowestEntropyElement.Position;
            lowestEntropySet.Remove(lowestEntropyElement);
            return returnVEctor;
        }

    }
}