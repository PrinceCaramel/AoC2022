using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day1 : IDay
    {
        #region Fields

        List<int> mCalories = new List<int>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            return this.mCalories.Max().ToString();
        }

        public string GetSecondPuzzle()
        {
            this.mCalories = this.mCalories.OrderDescending().ToList();
            return (this.mCalories[0] + this.mCalories[1] + this.mCalories[2]).ToString();
        }

        public void ComputesData()
        {
            int lCalorie = 0;
            IEnumerable<string> lInput = Utils.GetInputData(this, true);
            foreach (string lLine in lInput)
            {
                int lResult;
                if (int.TryParse(lLine, out lResult))
                {
                    lCalorie += lResult;
                }
                else
                {
                    this.mCalories.Add(lCalorie);
                    lCalorie = 0;
                }
            }
        }

        #endregion Methods
    }
}
