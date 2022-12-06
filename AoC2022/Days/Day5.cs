using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day5 : IDay
    {
        #region Fields

        List<Tuple<int, int, int>> mInstructions = new List<Tuple<int, int, int>>();
        List<Stack<string>> mStacks = new List<Stack<string>>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            foreach (Tuple<int,int,int> lInstruction in this.mInstructions)
            {
                for (int lIndex = 0; lIndex < lInstruction.Item1; lIndex++)
                {
                    this.mStacks.ElementAt(lInstruction.Item3 - 1).Push(this.mStacks.ElementAt(lInstruction.Item2 - 1).Pop());
                }
            }
            return this.GetTopResult();
        }

        public string GetSecondPuzzle()
        {
            this.mInstructions.Clear();
            this.mStacks.Clear();
            this.ComputesData();
            List<string> lTemp = new List<string>();
            foreach (Tuple<int, int, int> lInstruction in this.mInstructions)
            {
                lTemp.Clear();
                for (int lIndex = 0; lIndex < lInstruction.Item1; lIndex++)
                {
                    lTemp.Add(this.mStacks.ElementAt(lInstruction.Item2 - 1).Pop());
                }
                lTemp.Reverse();
                foreach(string lCrate in lTemp)
                {
                    this.mStacks.ElementAt(lInstruction.Item3 - 1).Push(lCrate);
                }
            }
            return this.GetTopResult();
        }

        public void ComputesData()
        {
            IEnumerable<string> lInput = Utils.GetInputData(this);
            bool lIsIntruction = false;
            int lNumberOfStacks = (lInput.First().Length + 1) / 4;
            for (int lIndex = 0; lIndex < lNumberOfStacks; lIndex++)
            {
                this.mStacks.Add(new Stack<string>());
            }
            List<string> lStacks = new List<string>();
            foreach (string lLine in lInput)
            {
                if (string.IsNullOrEmpty(lLine))
                {
                    lIsIntruction = true;
                    lStacks.Reverse();
                    lStacks.RemoveAt(0);
                    foreach (string lStackLine in lStacks)
                    {
                        for (int lIndex = 0; lIndex < lNumberOfStacks; lIndex++)
                        {
                            char lChar = lStackLine[lIndex * 4 + 1];
                            if (!lChar.Equals(' '))
                            {
                                this.mStacks.ElementAt(lIndex).Push(lChar.ToString());
                            }
                        }
                    }
                }
                else
                {
                    if (lIsIntruction)
                    {
                        string[] lSplit = lLine.Split(' ');
                        int lNumberOfCrates;
                        int lFrom;
                        int lTo;
                        int.TryParse(lSplit[1], out lNumberOfCrates);
                        int.TryParse(lSplit[3], out lFrom);
                        int.TryParse(lSplit[5], out lTo);
                        this.mInstructions.Add(new Tuple<int, int, int>(lNumberOfCrates, lFrom, lTo));
                    }
                    else
                    {
                        lStacks.Add(lLine);
                    }
                }
            }
        }

        private string GetTopResult()
        {
            string lResult = "";
            foreach (Stack<string> lStack in this.mStacks)
            {
                lResult += lStack.First();
            }
            return lResult;
        }

        #endregion Methods
    }
}
