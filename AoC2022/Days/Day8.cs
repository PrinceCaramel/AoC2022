using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AoC2022.Days
{
    public class Day8 : IDay
    {
        #region Fields

        private List<List<int>> mMap = new List<List<int>>();
        private int mWidth;
        private int mHeight;

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            int lCount = this.GetCountPerimeter();
            for (int lX = 1; lX < this.mWidth - 1; lX++)
            {
                for (int lY = 1; lY < this.mHeight - 1; lY++)
                {
                    lCount += this.IsVisible(lX, lY) ? 1 : 0;
                }
            }
            return lCount.ToString();
        }

        public string GetSecondPuzzle()
        {
            int lMax = 0;
            for (int lX = 1; lX < this.mWidth - 1; lX++)
            {
                for (int lY = 1; lY < this.mHeight - 1; lY++)
                {
                    lMax = Math.Max(lMax, this.GetScenicScore(lX, lY));
                }
            }
            return lMax.ToString();
        }

        public void ComputesData()
        {
            IEnumerable<string> lInput = Utils.GetInputData(this);
            foreach (string lLine in lInput) 
            {
                List<int> lTreeLine = lLine.ToCharArray().Select(pChar => int.Parse(pChar.ToString())).ToList();
                this.mMap.Add(lTreeLine);
            }
            this.mHeight = this.mMap.Count;
            this.mWidth = this.mMap.First().Count;
        }

        private bool IsVisible(int pX, int pY)
        {
            bool lIsVisible = false;
            int lValue = this.mMap.ElementAt(pY).ElementAt(pX);
            //Left
            lIsVisible |= this.mMap.ElementAt(pY).GetRange(0, pX).All(pTree => pTree < lValue);
            //Right
            if (!lIsVisible)
            {
                lIsVisible |= this.mMap.ElementAt(pY).GetRange(pX + 1, this.mWidth - 1 - pX).All(pTree => pTree < lValue);
            }
            //Top
            if (!lIsVisible)
            {
                lIsVisible |= this.mMap.GetRange(0, pY).Select(pLine => pLine.ElementAt(pX)).All(pTree => pTree < lValue);
            }
            //Bottom
            if (!lIsVisible)
            {
                lIsVisible |= this.mMap.GetRange(pY + 1, this.mHeight - 1 - pY).Select(pLine => pLine.ElementAt(pX)).All(pTree => pTree < lValue);
            }

            return lIsVisible;
        }

        private int GetScenicScore(int pX, int pY) 
        {
            int lValue = this.mMap.ElementAt(pY).ElementAt(pX);
            int lScore = 1;
            //Left
            List<int> lTemp = this.mMap.ElementAt(pY).GetRange(0, pX).ToList();
            lTemp.Reverse();
            lScore = lScore * this.GetAmountOfLowerTrees(lValue, lTemp);

            //Right
            lTemp = this.mMap.ElementAt(pY).GetRange(pX + 1, this.mWidth - 1 - pX).ToList();
            lScore = lScore * this.GetAmountOfLowerTrees(lValue, lTemp);

            //Top
            lTemp = this.mMap.GetRange(0, pY).Select(pLine => pLine.ElementAt(pX)).ToList();
            lTemp.Reverse();
            lScore = lScore * this.GetAmountOfLowerTrees(lValue, lTemp);

            //Bottom
            lTemp = this.mMap.GetRange(pY + 1, this.mHeight - 1 - pY).Select(pLine => pLine.ElementAt(pX)).ToList();
            lScore = lScore * this.GetAmountOfLowerTrees(lValue, lTemp);

            return lScore;
        }

        private int GetAmountOfLowerTrees(int pValue, List<int> pTreeRange) 
        {
            int lAmount = 0;
            if (pTreeRange.Any(pTree => pTree >= pValue))
            {
                lAmount = pTreeRange.IndexOf(pTreeRange.First(pTree => pTree >= pValue)) + 1;
            }
            else
            {
                lAmount = pTreeRange.Count;
            }
            return lAmount;
        }

        private int GetCountPerimeter()
        {
            return this.mWidth * 2 + (this.mHeight - 2) * 2;
        }

        #endregion Methods
    }
}
