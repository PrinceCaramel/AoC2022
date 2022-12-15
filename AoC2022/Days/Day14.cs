using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day14 : IDay
    {
        #region Fields

        private string[] mSeparator = (new List<string>() { " -> "}).ToArray();
        private List<Coord> mRocks = new List<Coord>();
        private List<Coord> mRocksAndSand = new List<Coord>();
        private int mRocksDepth = 0;
        private int mRocksMinX = 500;
        private int mRocksMaxX = 500;
        private Coord mSource;

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            bool lContinueDroppingSand = true;
            while (lContinueDroppingSand)
            {
                lContinueDroppingSand = !this.DropSandAndReturnsTrueIfInfiniteFall();
            }
            return (this.mRocksAndSand.Count() - this.mRocks.Count()).ToString();
        }

        public string GetSecondPuzzle()
        {
            bool lContinueDroppingSand = true;
            while (lContinueDroppingSand)
            {
                lContinueDroppingSand = !this.DropSandAndReturnsTrueIfMax();
            }
            return (this.mRocksAndSand.Count() - this.mRocks.Count()).ToString();
        }

        public void ComputesData()
        {
            List<string> lData = Utils.GetInputData(this).ToList();
            foreach (string lLine in lData)
            {
                this.ComputeLineOfRocks(lLine);
            }
            this.mRocks = this.mRocks.Distinct().ToList();
            this.mRocksAndSand = this.mRocks.ToList();
            this.mSource = new Coord(500, 0);
            this.mRocksDepth = this.mRocks.Select(pCoord => pCoord.Y).Max();
            IEnumerable<int> lXs = this.mRocks.Select(pCoord => pCoord.X);
            this.mRocksMaxX = lXs.Max();
            this.mRocksMinX = lXs.Min();
        }

        private void ComputeLineOfRocks(string pLine)
        {
            Coord[] lCoordinates = pLine.Split(this.mSeparator, StringSplitOptions.RemoveEmptyEntries).Select(pStr => Coord.CreateFromString(pStr)).ToArray();
            for (int lIndex = 0; lIndex < lCoordinates.Count() - 1; lIndex++)
            {
                this.GetLine(lCoordinates[lIndex], lCoordinates[lIndex + 1]).ForEach(pRock => this.mRocks.Add(pRock));
            }
        }

        private List<Coord> GetLine(Coord pLeft, Coord pRight)
        {
            List<Coord> lResult = new List<Coord>();
            if (pLeft.X == pRight.X)
            {
                int lMin = Math.Min(pLeft.Y, pRight.Y);
                int lCount = Math.Abs(pLeft.Y - pRight.Y) + 1;
                Enumerable.Range(lMin, lCount).ToList().ForEach(pY => lResult.Add(new Coord(pLeft.X, pY)));
            }
            else
            {
                int lMin = Math.Min(pLeft.X, pRight.X);
                int lCount = Math.Abs(pLeft.X - pRight.X) + 1;
                Enumerable.Range(lMin, lCount).ToList().ForEach(pX => lResult.Add(new Coord(pX, pLeft.Y)));
            }
            return lResult;
        }

        private bool DropSandAndReturnsTrueIfInfiniteFall()
        {
            bool lIsInifiniteFall = false;
            Coord lSand = this.mSource;
            bool lContinueProcess = true;
            while (lContinueProcess)
            {
                lSand.Fall();
                if (this.mRocksAndSand.Contains(lSand))
                {
                    lSand.RollLeft();
                    if (this.mRocksAndSand.Contains(lSand))
                    {
                        lSand.RollRight();
                        lSand.RollRight();
                        if (this.mRocksAndSand.Contains(lSand))
                        {
                            lSand.RollLeft();
                            lSand.GoUp();
                            this.mRocksAndSand.Add(lSand);
                            lContinueProcess = false;
                        }
                    }
                }
                else
                {
                    if (lSand.Y > this.mRocksDepth)
                    {
                        lContinueProcess = false;
                        lIsInifiniteFall = true;
                    }
                }
            }
            return lIsInifiniteFall;

        }

        private bool DropSandAndReturnsTrueIfMax()
        {
            bool lMaxReached = false;
            Coord lSand = this.mSource;
            bool lContinueProcess = true;
            while (lContinueProcess)
            {
                lSand.Fall();
                if (this.mRocksAndSand.Contains(lSand))
                {
                    lSand.RollLeft();
                    if (this.mRocksAndSand.Contains(lSand))
                    {
                        lSand.RollRight();
                        lSand.RollRight();
                        if (this.mRocksAndSand.Contains(lSand))
                        {
                            lSand.RollLeft();
                            lSand.GoUp();
                            this.mRocksAndSand.Add(lSand);
                            if (lSand.Equals(this.mSource))
                            {
                                lMaxReached = true;
                            }
                            lContinueProcess = false;
                        }
                    }
                }
                if (lSand.Y == this.mRocksDepth + 1)
                {
                    lContinueProcess = false;
                    this.mRocksAndSand.Add(lSand);
                }
            }
            return lMaxReached;
        }

        #endregion Methods

        public struct Coord
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public Coord(int pX, int pY)
            {
                this.X = pX;
                this.Y = pY;
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}", this.X, this.Y);
            }

            public static Coord CreateFromString(string pCoord)
            {
                string[] lSplit = pCoord.Split(',');
                return new Coord(int.Parse(lSplit[0]), int.Parse(lSplit[1]));
            }

            public void Fall()
            {
                this.Y++;
            }

            public void GoUp()
            {
                this.Y--;
            }

            public void RollLeft()
            {
                this.X--;
            }

            public void RollRight()
            {
                this.X++;
            }
        }
    }

}
