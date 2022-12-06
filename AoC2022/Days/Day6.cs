using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day6 : IDay
    {
        #region Fields

        List<string> mCodes = new List<string>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            string lCode = this.mCodes.First();
            return this.GetIndexOfMarker(lCode, 4).ToString();
        }

        public string GetSecondPuzzle()
        {
            string lCode = this.mCodes.First();
            return this.GetIndexOfMarker(lCode, 14).ToString();
        }

        public void ComputesData()
        {
            this.mCodes = Utils.GetInputData(this).ToList();
        }

        private List<char> GetListOfChar(string pString, int pValue)
        {
            return pString.Substring(0, pValue).ToCharArray().ToList();
        }

        private int GetIndexOfMarker(string pCode, int pStartIndex)
        {
            List<char> lChars = this.GetListOfChar(pCode, pStartIndex);
            for (int lIndex = pStartIndex; lIndex < pCode.Length; lIndex++)
            {
                if (lChars.Distinct().Count() != lChars.Count())
                {
                    lChars.RemoveAt(0);
                    lChars.Add(pCode[lIndex]);
                }
                else
                {
                    return lIndex;
                }
            }
            return -1;
        }

        #endregion Methods
    }
}
