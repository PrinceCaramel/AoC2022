using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day4 : IDay
    {
        #region Fields

        List<string> mPairs = new List<string>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            int lResult = 0;
            foreach (string lPair in this.mPairs)
            {
                Tuple<int,int,int,int> lTuple = this.GetTupleFromLine(lPair);
                lResult += (lTuple.Item1 >= lTuple.Item3 && lTuple.Item2 <= lTuple.Item4 ||
                            lTuple.Item3 >= lTuple.Item1 && lTuple.Item4 <= lTuple.Item2) ? 1 : 0;
            }
            return lResult.ToString();
        }

        public string GetSecondPuzzle()
        {
            int lResult = 0;
            foreach (string lPair in this.mPairs)
            {
                Tuple<int, int, int, int> lTuple = this.GetTupleFromLine(lPair);
                lResult += (lTuple.Item1 <= lTuple.Item4 && lTuple.Item2 >= lTuple.Item3 ||
                            lTuple.Item3 <= lTuple.Item2 && lTuple.Item4 >= lTuple.Item1) ? 1 : 0;
            }
            return lResult.ToString();
        }

        public void ComputesData()
        {
            this.mPairs = Utils.GetInputData(this).ToList();
        }

        private Tuple<int,int,int,int> GetTupleFromLine(string pLine)
        {
            string[] lElvesPair = pLine.Split(',');
            string lFirst = lElvesPair[0];
            string lSecond = lElvesPair[1];
            int lFirstLeft;
            int lFirstRight;
            int lSecondLeft;
            int lSecondRight;
            int.TryParse(lFirst.Split('-')[0], out lFirstLeft);
            int.TryParse(lFirst.Split('-')[1], out lFirstRight);
            int.TryParse(lSecond.Split('-')[0], out lSecondLeft);
            int.TryParse(lSecond.Split('-')[1], out lSecondRight);

            return new Tuple<int, int, int, int>(lFirstLeft, lFirstRight, lSecondLeft, lSecondRight);
        }

        #endregion Methods
    }
}
