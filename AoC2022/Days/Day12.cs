using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day12 : IDay
    {
        //409 too high
        #region Fields

        private List<string> mRawData = new List<string>();
        private Dictionary<string, int> mGraphWithValue = new Dictionary<string, int>();
        private List<string> mGraphNodes = new List<string>();
        private Dictionary<string, int> mGraphNodesToDistance = new Dictionary<string, int>();
        private Dictionary<string, List<string>> mNeighbors = new Dictionary<string, List<string>>();
        private Dictionary<string, string> mFinalPath = new Dictionary<string, string>();
        private string mEndNode = "";
        private string mStartNode = "";

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            this.Dijkstra();
            return this.Result().ToString();
        }

        public string GetSecondPuzzle()
        {
            return "";
        }

        public void ComputesData()
        {
            this.mRawData = Utils.GetInputData(this).ToList();
            int lMaxX = this.mRawData.First().Length;
            int lMaxY = this.mRawData.Count;

            for (int lX = 0; lX < lMaxX; lX++)
            {
                for (int lY = 0; lY < lMaxY; lY++)
                {
                    char lChar = this.mRawData[lY][lX];
                    if (lChar == 'S')
                    {
                        lChar = 'a';
                        this.mStartNode = this.GetId(lX, lY);
                    }
                    if (lChar == 'E')
                    {
                        lChar = 'z';
                        this.mEndNode = this.GetId(lX,lY);
                    }

                    string lId = this.GetId(lX, lY);
                    this.mGraphWithValue.Add(lId, 1);
                    this.mGraphNodes.Add(lId);
                    this.mNeighbors.Add(lId, new List<string>());
                    foreach (Tuple<int,int> lNeighbor in Utils.GetNeighbors(lX, lY, lMaxX - 1, lMaxY - 1))
                    {
                        if (lChar >= this.GetChar(lNeighbor.Item1, lNeighbor.Item2) - 1)
                        {
                            this.mNeighbors[lId].Add(this.GetId(lNeighbor.Item1, lNeighbor.Item2));
                        }
                    }
                }
            }
            this.mNeighbors[this.mEndNode].Clear();
            this.mGraphNodesToDistance.Add(this.mStartNode, 0);
        }

        private char GetChar(int pX, int pY)
        {
            char lChar = this.mRawData[pY][pX];
            if (lChar == 'S')
                lChar = 'a';
            if (lChar == 'E')
                lChar = 'z';
            return lChar;
        }

        private string FindMinimum()
        {
            int lMinimum = int.MaxValue;
            string lResultNode = this.GetId(-1, -1);
            foreach (string lNode in this.mGraphNodes)
            {
                int lValue;
                if (this.mGraphNodesToDistance.TryGetValue(lNode, out lValue))
                {
                    if (lValue < lMinimum)
                    {
                        lMinimum = lValue;
                        lResultNode = lNode;
                    }
                }
            }
            if (lResultNode == "-1,-1")
            {
                Console.WriteLine("");
            }
            return lResultNode;
        }

        private void UpdateDistance(string pNode1, string pNode2)
        {
            int lDistanceNode1 = this.mGraphNodesToDistance[pNode1];
            int lDistanceNode2;
            bool lExist = true;
            if (!this.mGraphNodesToDistance.TryGetValue(pNode2, out lDistanceNode2))
            {
                lDistanceNode2 = int.MaxValue;
                lExist = false;
            }
            int lValue = this.mGraphWithValue[pNode1];
            if (lDistanceNode2 > (lDistanceNode1 + lValue))
            {
                if (lExist)
                {
                    this.mGraphNodesToDistance[pNode2] = lDistanceNode1 + lValue;
                }
                else
                {
                    this.mGraphNodesToDistance.Add(pNode2, lDistanceNode1 + lValue);
                }
                this.AddPredecessor(pNode2, pNode1);
            }
        }

        private void Dijkstra()
        {
            while (this.mGraphNodes.Any())
            {
                string lNode = this.FindMinimum();
                this.mGraphNodes.Remove(lNode);
                List<string> lNeighbors = this.mNeighbors[lNode];
                lNeighbors.ForEach(pNb => this.UpdateDistance(lNode, pNb));
            }
        }

        private string GetId(int pX, int pY)
        {
            return string.Format("{0},{1}", pX, pY);
        }

        private void AddPredecessor(string pNode, string pPredecessor)
        {
            string lOutput;
            if (this.mFinalPath.TryGetValue(pNode, out lOutput))
            {
                this.mFinalPath[pNode] = pPredecessor;
            }
            else
            {
                this.mFinalPath.Add(pNode, pPredecessor);
            }
        }

        private string Result()
        {
            Stack<string> lPathValues = new Stack<string>();
            string lEndNode = this.mEndNode;
            string lStartNode = this.mStartNode;
            while (!lEndNode.Equals(lStartNode))
            {
                lPathValues.Push(this.mFinalPath[lEndNode]);
                lEndNode = this.mFinalPath[lEndNode];
            }
            return lPathValues.Count.ToString();
        }

        #endregion Methods
    }
}
