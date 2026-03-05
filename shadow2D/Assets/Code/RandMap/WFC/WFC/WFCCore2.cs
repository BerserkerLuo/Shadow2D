using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC
{
    public class WFCCore2
    {
        OutputGrid outputGrid;

        int maxIterations = 1000;

        public WFCCore2(int Width, int Height) {
            outputGrid = new OutputGrid(Width, Height);
        }

        public int[][] CreateGrid() {
             
            int iteration = 0;
            while (iteration < this.maxIterations)
            {
                iteration++;
                int innerIteration = 10000;
                Solver solver = new Solver(outputGrid);
                while (!solver.CheckForConflics() && !solver.CheckIfSolved())
                {
                    solver.CollapseLowestEntropyCell();
                    solver.Propagate();

                    innerIteration--;
                    if (innerIteration <= 0)
                    {
                        Debug.Log("Propagation taking too long");
                        return new int[0][];
                    }
                    if (solver.CheckForConflics())
                    {
                        Debug.Log("\nCOnflict occured. Iteration: " + iteration);
                        outputGrid.ResetAllPossibilities();
                        solver = new Solver(this.outputGrid);
                    }
                    else
                    {
                        Debug.Log("Solved on " + iteration + " iteration");
                        outputGrid.PrintResultsToConsol();
                        break;
                    }
                }
            }

            if (iteration == this.maxIterations)
                Debug.Log("CreateGrid Faild !");
            else
                Debug.Log("CreateGrid Succ !");


            return outputGrid.GetSolvedOutputGrid();
        }

        public int[][] ExcuteTick() {

            int innerIteration = 1000;
            Solver solver = new Solver(outputGrid);
            while (!solver.CheckForConflics() && !solver.CheckIfSolved())
            {
                solver.CollapseLowestEntropyCell();
                solver.Propagate();

                innerIteration--;
                if (innerIteration <= 0)
                {
                    Debug.Log("Propagation taking too long");
                    return new int[0][];
                }
                if (solver.CheckForConflics())
                {
                    outputGrid.ResetAllPossibilities();
                    solver = new Solver(this.outputGrid);
                }
                else
                {
                    outputGrid.PrintResultsToConsol();
                    break;
                }
            }

            return outputGrid.GetSolvedOutputGrid();
        }

        public Dictionary<int, HashSet<int>> GetPossibleValues() {
            return outputGrid.GetPossibleValues();
        }
    }


}