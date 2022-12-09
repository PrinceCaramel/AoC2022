using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day9 : IDay
    {
        #region Fields

        List<Move> mMoves = new List<Move>();
        List<Position> mTailsVisitedPositions = new List<Position>();
        List<Position> mRopeKnotsPosition = new List<Position>();

        #endregion Fields

        #region Properties

        public Position Tail
        {
            get
            {
                return this.mRopeKnotsPosition.Last();
            }
        }

        #endregion Properties

        #region Methods

        public string GetFirstPuzzle()
        {
            return this.ComputePuzzle(2);
        }

        public string GetSecondPuzzle()
        {
            return this.ComputePuzzle(10);
        }

        public void ComputesData()
        {
            IEnumerable<string> lInput = Utils.GetInputData(this);
            foreach (string lLine in lInput)
            {
                string[] lSplit = lLine.Split(' ');
                this.mMoves.Add(new Move((Direction)Enum.Parse(typeof(Direction), lSplit[0]), int.Parse(lSplit[1])));
            }
        }

        private string ComputePuzzle(int pKnots)
        {
            this.BuildRope(pKnots);
            this.mTailsVisitedPositions.Clear();
            this.AddTailPosition();
            foreach (Move lMove in this.mMoves)
            {
                this.ComputeMove(lMove);
            }
            return this.mTailsVisitedPositions.Count().ToString();
        }

        private void ComputeMove(Move pMove)
        {
            for (int lStep = 0; lStep < pMove.Steps; lStep++)
            {
                this.mRopeKnotsPosition[0] = this.mRopeKnotsPosition[0].Move(pMove.Direction);
                this.MoveKnots();
                this.AddTailPosition();
            }
        }

        private void BuildRope(int pKnot)
        {
            this.mRopeKnotsPosition.Clear();
            for (int lCount = 0; lCount < pKnot; lCount++)
            {
                this.mRopeKnotsPosition.Add(new Position(0, 0));
            }
        }

        private void MoveKnots()
        {
            for (int lKnotIndex = 1; lKnotIndex < this.mRopeKnotsPosition.Count; lKnotIndex++)
            {
                Position lCurrent = this.mRopeKnotsPosition.ElementAt(lKnotIndex);
                Position lNext = this.mRopeKnotsPosition.ElementAt(lKnotIndex - 1); // Next ahead of him.
                if (lCurrent.ShouldMove(lNext))
                {
                    this.mRopeKnotsPosition[lKnotIndex] = lCurrent.Follow(lNext);
                }
                else
                {
                    break;
                }
            }
            
        }

        private void AddTailPosition()
        {
            if (!this.mTailsVisitedPositions.Contains(this.Tail))
            {
                this.mTailsVisitedPositions.Add(this.Tail);
            }
        }

        #endregion Methods
    }

    public enum Direction
    {
        R,
        L,
        U,
        D
    }

    public struct Position
    {
        public Position(double pX, double pY)
        {
            this.X = pX;
            this.Y = pY;
        }

        public double X { get; }
        public double Y { get; }

        public static Position operator -(Position pPosition1, Position pPosition2) => new Position(pPosition1.X - pPosition2.X, pPosition1.Y - pPosition2.Y);

        public override string ToString()
        {
            return string.Format("x:{0} y:{1}", this.X, this.Y);
        }

        public Position Move(Direction pDirection)
        {
            switch (pDirection)
            {
                case Direction.R:
                    return new Position(this.X + 1, this.Y);
                case Direction.L:
                    return new Position(this.X - 1, this.Y);
                case Direction.U:
                    return new Position(this.X, this.Y + 1);
                case Direction.D:
                    return new Position(this.X, this.Y - 1);
                default:
                    return this;
            }
        }

        public bool ShouldMove(Position pNextBodyPart)
        {
            return Math.Abs(this.X - pNextBodyPart.X) >= 2 ||
                Math.Abs(this.Y - pNextBodyPart.Y) >= 2;
        }

        public Position Follow(Position pPositionToFollow)
        {
            Position lDifference = pPositionToFollow - this;
            //UR
            if (lDifference.Equals(new Position(1 , 2)) || lDifference.Equals(new Position(2, 1)) || lDifference.Equals(new Position(2, 2)))
            {
                return this.Move(Direction.U).Move(Direction.R);
            }
            //R
            else if (lDifference.Equals(new Position(2, 0)))
            {
                return this.Move(Direction.R);
            }
            //DR
            else if (lDifference.Equals(new Position(2, -1)) || lDifference.Equals(new Position(1, -2)) || lDifference.Equals(new Position(2, -2)))
            {
                return this.Move(Direction.D).Move(Direction.R);
            }
            //D
            else if (lDifference.Equals(new Position(0, -2)))
            {
                return this.Move(Direction.D);
            }
            //DL
            else if (lDifference.Equals(new Position(-1, -2)) || lDifference.Equals(new Position(-2, -1)) || lDifference.Equals(new Position(-2, -2)))
            {
                return this.Move(Direction.D).Move(Direction.L);
            }
            //L
            else if (lDifference.Equals(new Position(-2, 0)))
            {
                return this.Move(Direction.L);
            }
            //UL
            else if (lDifference.Equals(new Position(-2, 1)) || lDifference.Equals(new Position(-1, 2)) || lDifference.Equals(new Position(-2, 2)))
            {
                return this.Move(Direction.U).Move(Direction.L);
            }
            //U
            else if (lDifference.Equals(new Position(0, 2)))
            {
                return this.Move(Direction.U);
            }
            else
            {
                return this;
            }
        }
    }

    public struct Move
    {
        public Move(Direction pDirection, int pSteps)
        {
            this.Direction = pDirection;
            this.Steps = pSteps;
        }

        public Direction Direction { get; }
        public int Steps { get; }
    }
}
