using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static AoC2022.Days.HLine;

namespace AoC2022.Days
{
    public class Day17 : IDay
    {
        #region Fields

        private string mJetPushes;
        private int mJetLength;
        private int mCurrentPush = 0;

        #endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            return this.Run().ToString();
        }

        public string GetSecondPuzzle()
        {
            return "";
        }

        public void ComputesData()
        {
            this.mJetPushes = Utils.GetInputData(this, true).ToList().First();
            this.mJetLength = this.mJetPushes.Length;
        }

        private int Run()
        {
            RockShapePooler lPooler = new RockShapePooler();
            int lCurrentTopPoint = -1;
            List<Coord> lTower= new List<Coord>();
            for (int lCount = 0; lCount < 2023; lCount++)
            {
                ARockShape lRock = lPooler.GetNextRockShape();
                lRock.Init(lCurrentTopPoint);
                bool lContinueFalling = true;
                while (lContinueFalling)
                {
                    this.PushRock(lRock, lTower);
                    lContinueFalling = lRock.CanFall(lTower);
                    if (lContinueFalling)
                        lRock.Fall();
                }
                lCurrentTopPoint = Math.Max(lCurrentTopPoint, (int)lRock.TopY);
                lRock.AddShapeToGivenList(lTower);
            }
            return lCurrentTopPoint;
        }

        private void PushRock(ARockShape pRock, IEnumerable<Coord> pTower)
        {
            char lChar = this.mJetPushes[this.mCurrentPush % this.mJetLength];
            if (lChar == '>')
            {
                if (pRock.CanGoRight(pTower))
                    pRock.GoRight();
            }
            else
            {
                if (pRock.CanGoLeft(pTower))
                    pRock.GoLeft();
            }

            this.mCurrentPush++;
        }

        #endregion Methods
    }

    public class RockShapePooler
    {
        private int mCounter = -1;
        public ARockShape GetNextRockShape()
        {
            this.mCounter++;
            if (mCounter%5 == 0)
            {
                return new HLine();
            }
            else if (mCounter % 5 == 1)
            {
                return new Cross();
            }
            else if (mCounter % 5 == 2)
            {
                return new LShape();
            }
            else if (mCounter % 5 == 3)
            {
                return new VLine();
            }
            else
            {
                return new Square();
            }
        }
    }

    public class HLine : ARockShape
    {
        public override Int64 TopY => this.CurrentPosition.Y;
        public override bool CanGoLeft(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y));
        }

        public override bool CanGoRight(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X + 3 == 6)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X + 4, this.CurrentPosition.Y));
        }

        public override bool CanFall(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.Y  == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y - 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y - 1))&&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y - 1)) && 
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 3, this.CurrentPosition.Y - 1));
        }

        public override void AddShapeToGivenList(List<Coord> pRocks)
        {
            pRocks.Add(this.CurrentPosition);
            pRocks.Add(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y));
            pRocks.Add(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y));
            pRocks.Add(new Coord(this.CurrentPosition.X + 3, this.CurrentPosition.Y));
        }
    }

    public class Cross : ARockShape
    {
        public override Int64 TopY => this.CurrentPosition.Y + 2;
        public override bool CanGoLeft(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X - 1 == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X - 2, this.CurrentPosition.Y + 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y + 2));
        }

        public override bool CanGoRight(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X + 1 == 6)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y + 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 2));
        }

        public override bool CanFall(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.Y == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y - 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y));
        }

        public override void AddShapeToGivenList(List<Coord> pRocks)
        {
            pRocks.Add(this.CurrentPosition);
            pRocks.Add(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y + 1));
            pRocks.Add(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y + 1));
            pRocks.Add(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 1));
            pRocks.Add(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y + 2));
        }

        public override void Init(int pTopTowerPoint)
        {
            this.CurrentPosition = new Coord(3, pTopTowerPoint + 4);
        }
    }

    public class LShape : ARockShape
    {
        public override Int64 TopY => this.CurrentPosition.Y + 2;
        public override bool CanGoLeft(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 2));
        }

        public override bool CanGoRight(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X + 2 == 6)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X + 3, this.CurrentPosition.Y)) &&
                            !pRocks.Contains(new Coord(this.CurrentPosition.X + 3, this.CurrentPosition.Y + 1)) &&
                            !pRocks.Contains(new Coord(this.CurrentPosition.X + 3, this.CurrentPosition.Y + 2));
        }

        public override bool CanFall(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.Y == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y - 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y - 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y - 1));
        }

        public override void AddShapeToGivenList(List<Coord> pRocks)
        {
            pRocks.Add(this.CurrentPosition);
            pRocks.Add(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y));
            pRocks.Add(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y));
            pRocks.Add(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y + 1));
            pRocks.Add(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y + 2));
        }
    }

    public class VLine : ARockShape
    {
        public override Int64 TopY => this.CurrentPosition.Y + 3;
        public override bool CanGoLeft(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y+1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y+2)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y+3));
        }

        public override bool CanGoRight(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X == 6)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y)) &&
                    !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 1)) &&
                    !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 2)) &&
                    !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 3));
        }

        public override bool CanFall(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.Y == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y - 1));
        }

        public override void AddShapeToGivenList(List<Coord> pRocks)
        {
            pRocks.Add(this.CurrentPosition);
            pRocks.Add(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y + 1));
            pRocks.Add(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y + 2));
            pRocks.Add(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y + 3));
        }
    }

    public class Square : ARockShape
    {
        public override Int64 TopY => this.CurrentPosition.Y + 1;
        public override bool CanGoLeft(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y + 1));
        }

        public override bool CanGoRight(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.X == 6)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 2, this.CurrentPosition.Y + 1));
        }

        public override bool CanFall(IEnumerable<Coord> pRocks)
        {
            if (this.CurrentPosition.Y == 0)
                return false;
            return !pRocks.Contains(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y - 1)) &&
                !pRocks.Contains(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y - 1));
        }

        public override void AddShapeToGivenList(List<Coord> pRocks)
        {
            pRocks.Add(this.CurrentPosition);
            pRocks.Add(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y));
            pRocks.Add(new Coord(this.CurrentPosition.X, this.CurrentPosition.Y + 1));
            pRocks.Add(new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y + 1));
        }
    }

    public abstract class ARockShape
    {
        public abstract Int64 TopY { get; }
        public Coord CurrentPosition { get; protected set; }
        public abstract bool CanGoLeft(IEnumerable<Coord> pRocks);
        public abstract bool CanGoRight(IEnumerable<Coord> pRocks);
        public abstract bool CanFall(IEnumerable<Coord> pRocks);
        public void Fall()
        {
            this.CurrentPosition = new Coord(this.CurrentPosition.X, this.CurrentPosition.Y - 1);
        }

        public void GoLeft()
        {
            this.CurrentPosition = new Coord(this.CurrentPosition.X - 1, this.CurrentPosition.Y);
        }

        public void GoRight()
        {
            this.CurrentPosition = new Coord(this.CurrentPosition.X + 1, this.CurrentPosition.Y);
        }

        public virtual void Init(int pTopTowerPoint)
        {
            this.CurrentPosition = new Coord(2, pTopTowerPoint + 4);
        }

        public abstract void AddShapeToGivenList(List<Coord> pRocks);
    }
}
