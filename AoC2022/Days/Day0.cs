using AoC2022.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AoC2022.Utilities;

namespace AoC2022.Days
{
    public class Day0 : IDay
    {
        List<Coord> mNodesG = new List<Coord>();
        Dictionary<Coord, int> mDistancesD = new Dictionary<Coord, int>(); 

        #region Methods

        public string GetFirstPuzzle()
        {
            this.Dijkstra();
            IEnumerable<Coord> lResult = this.Result();
            return (lResult.Count() - 1).ToString();
        }

        public string GetSecondPuzzle()
        {
            return "SecondPuzzle";
        }

        public void ComputesData()
        {
            this.mRawData = Utils.GetInputData(new Day12()).ToList();
            int lMaxX = this.mRawData.First().Length;
            int lMaxY = this.mRawData.Count;

            for (Int64 lX = 0; lX < lMaxX; lX++)
            {
                for (Int64 lY = 0; lY < lMaxY; lY++)
                {
                    Coord lCurrentNode = new Coord(lX, lY);
                    char lChar = this.mRawData[(int)lY][(int)lX];
                    if (lChar == 'S')
                    {
                        lChar = 'a';
                        this.mStartNode = lCurrentNode;
                    }
                    if (lChar == 'E')
                    {
                        lChar = 'z';
                        this.mEndNode = lCurrentNode;
                    }

                    
                    this.mEdges.Add(lCurrentNode, new List<Coord>());
                    foreach (Coord lNeighbor in lCurrentNode.GetNeighbors(0, 0, lMaxX - 1, lMaxY - 1))
                    {
                        if (lChar >= this.GetChar(lNeighbor) - 1)
                        {
                            this.mEdges[lCurrentNode].Add(lNeighbor);
                        }
                    }

                    //if (this.mEdges[lCurrentNode].Any())
                    {
                        this.mNodesG.Add(lCurrentNode);
                    }
                }
            }
        }

        private char GetChar(Coord pCoord)
        {
            char lChar = this.mRawData[(int)pCoord.Y][(int)pCoord.X];
            if (lChar == 'S')
                lChar = 'a';
            if (lChar == 'E')
                lChar = 'z';
            return lChar;
        }

        private void Initialization()
        {
            foreach (Coord lCoord in mNodesG)
            {
                this.mDistancesD.Add(lCoord, int.MaxValue);
            }
            this.mDistancesD[Coord.Origin] = 0;
        }

        private List<Coord> mQ = new List<Coord>();
        
        private Coord FindMinimum()
        {
            int lMin = int.MaxValue;
            Coord lNode = Coord.InvalidValue;
            foreach (Coord lCoord in this.mQ) 
            {
                if (this.mDistancesD[lCoord] < lMin)
                {
                    lMin = this.mDistancesD[lCoord];
                    lNode = lCoord;
                }
            }
            if (lNode.IsValid == false)
            {
                Console.WriteLine("");
            }
            return lNode;
        }

        private int Weight(Coord pS1, Coord pS2)
        {
            return 1;
        }

        private Dictionary<Coord, Coord> mPredecessor = new Dictionary<Coord, Coord>();
        private Dictionary<Coord, List<Coord>> mEdges = new Dictionary<Coord, List<Coord>>();
        private Coord mEndNode = Coord.InvalidValue;
        private Coord mStartNode = Coord.InvalidValue;
        private List<string> mRawData;

        private void UpdateDistance(Coord pS1, Coord pS2)
        {
            if (this.mDistancesD[pS2] > (this.mDistancesD[pS1] + Weight(pS1, pS2)))
            {
                this.mDistancesD[pS2] = this.mDistancesD[pS1] + Weight(pS1, pS2);
                this.mPredecessor[pS2] = pS1;
            }
        }
        
        private void Dijkstra()
        {
            this.Initialization();
            this.mQ = this.mNodesG.ToList();
            while (this.mQ.Any())
            {
                Coord lS1 = this.FindMinimum();
                this.mQ.Remove(lS1);
                foreach (Coord lS2 in this.mEdges[lS1])
                {
                    this.UpdateDistance(lS1, lS2);
                }
            }
        }

        private IEnumerable<Coord> Result()
        {
            Stack<Coord> lPathValues = new Stack<Coord>();
            Coord lEndNode = this.mEndNode;
            Coord lStartNode = Coord.Origin;
            while (!lEndNode.Equals(lStartNode))
            {
                lPathValues.Push(lEndNode);
                lEndNode = this.mPredecessor[lEndNode];
            }
            lPathValues.Push(lStartNode);
            return lPathValues;
        }

        #endregion Methods
    }
}
