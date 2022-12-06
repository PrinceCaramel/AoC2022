using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day3 : IDay
    {
        #region Fields

        List<string> mRuckSacks = new List<string>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            return this.mRuckSacks.Select(pSack => this.CharToInt(pSack.Substring(0, pSack.Length/2).First(pChar => pSack.Substring(pSack.Length / 2, pSack.Length / 2).Contains(pChar)))).Sum().ToString() ;
        }

        public string GetSecondPuzzle()
        {
            int lResult = 0;
            for (int lIndex = 0; lIndex < this.mRuckSacks.Count() / 3; lIndex++)
            {
                lResult += this.CharToInt(this.mRuckSacks[lIndex*3].First(pChar => this.mRuckSacks[(lIndex * 3) + 1].Contains(pChar) && this.mRuckSacks[(lIndex * 3) + 2].Contains(pChar)));
            }
            return lResult.ToString();
        }

        public void ComputesData()
        {
            this.mRuckSacks = Utils.GetInputData(this).ToList();
        }

        private int CharToInt(char pCharacter)
        {
            if (char.IsUpper(pCharacter))
            {
                return (int)pCharacter - 38;
            }
            return (int)pCharacter - 96;
        }

        #endregion Methods
    }
}
