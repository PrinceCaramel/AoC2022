using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day18 : IDay
    {
        #region Fields

        List<Coord3D> mCubes = new List<Coord3D>();
        int mMinX, mMinY, mMaxX, mMaxY, mMinZ, mMaxZ;
        Stack<CoordFill> mTempStack = new Stack<CoordFill>();
        List<Coord3D> mFilled = new List<Coord3D>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            return this.ComputeFaces(this.mCubes).ToString();
        }

        public string GetSecondPuzzle()
        {
            Coord3D lMinimum = new Coord3D(this.mMinX, this.mMinY, this.mMinZ);
            this.mFilled.Add(lMinimum);
            this.mTempStack.Push(new CoordFill(lMinimum, Fill.N));
            while (this.mTempStack.Any())
            {
                CoordFill lHead = this.mTempStack.Pop();
                if (!lHead.IsTerminated)
                {
                    this.mTempStack.Push(lHead.Update());
                }
                CoordFill lHeadSuccessor = lHead.GetSuccessor();
                if (lHeadSuccessor.Coord.X >= this.mMinX && lHeadSuccessor.Coord.X <= this.mMaxX &&
                    lHeadSuccessor.Coord.Y >= this.mMinY && lHeadSuccessor.Coord.Y <= this.mMaxY &&
                    lHeadSuccessor.Coord.Z >= this.mMinZ && lHeadSuccessor.Coord.Z <= this.mMaxZ &&
                    !this.mFilled.Contains(lHeadSuccessor.Coord) &&
                    !this.mCubes.Contains(lHeadSuccessor.Coord))
                {
                    this.mFilled.Add(lHeadSuccessor.Coord);
                    this.mTempStack.Push(lHeadSuccessor);
                }
            }
            int lXLength = Math.Abs(this.mMaxX - this.mMinX) + 1;
            int lYLength = Math.Abs(this.mMaxY - this.mMinY) + 1;
            int lZLength = Math.Abs(this.mMaxZ - this.mMinZ) + 1;
            int lAroundFaces = lXLength * lYLength * 2 + lXLength * lZLength * 2 + lYLength * lZLength * 2;
            return (this.ComputeFaces(this.mFilled) - lAroundFaces).ToString();
        }

        private int ComputeFaces(IEnumerable<Coord3D> pFaces)
        {
            int lFaces = 0;
            foreach (Coord3D lCoord in pFaces)
            {
                IEnumerable<Coord3D> lIntersect = pFaces.Intersect(lCoord.GetNeighbors(int.MinValue, int.MinValue, int.MinValue, int.MaxValue, int.MaxValue, int.MaxValue));
                lFaces += (6 - lIntersect.Count());
            }
            return lFaces;
        }

        public void ComputesData()
        {
            this.mCubes = Utils.GetInputData(this).Select(pCube => Coord3D.GetFromString(pCube)).ToList();
            this.mMinX = this.mCubes.Select(pCube => (int)pCube.X).Min() - 1;
            this.mMinY = this.mCubes.Select(pCube => (int)pCube.Y).Min() - 1;
            this.mMinZ = this.mCubes.Select(pCube => (int)pCube.Z).Min() - 1;
            this.mMaxX = this.mCubes.Select(pCube => (int)pCube.X).Max() + 1;
            this.mMaxY = this.mCubes.Select(pCube => (int)pCube.Y).Max() + 1;
            this.mMaxZ = this.mCubes.Select(pCube => (int)pCube.Z).Max() + 1;
        }

        public struct CoordFill
        {
            public Coord3D Coord;
            public Fill Fill;
            public bool IsTerminated { get => this.Fill == Fill.D; }
            public CoordFill(Coord3D pCoord, Fill pFill)
            {
                this.Coord = pCoord;
                this.Fill = pFill;
            }
            public CoordFill Update()
            {
                switch (this.Fill)
                {
                    case Fill.N:
                        return new CoordFill(this.Coord, Fill.S);
                    case Fill.S:
                        return new CoordFill(this.Coord, Fill.E);
                    case Fill.E:
                        return new CoordFill(this.Coord, Fill.W);
                    case Fill.W:
                        return new CoordFill(this.Coord, Fill.U);
                    case Fill.U:
                        return new CoordFill(this.Coord, Fill.D);
                    case Fill.D:
                        return new CoordFill(Coord3D.InvalidValue, Fill.D);
                    default:
                        return new CoordFill(Coord3D.InvalidValue, Fill.D);
                }
            }

            public CoordFill GetSuccessor()
            {
                switch (this.Fill)
                {
                    case Fill.N:
                        return new CoordFill(new Coord3D(this.Coord.X, this.Coord.Y - 1, this.Coord.Z), Fill.N);
                    case Fill.S:
                        return new CoordFill(new Coord3D(this.Coord.X, this.Coord.Y + 1, this.Coord.Z), Fill.N);
                    case Fill.E:
                        return new CoordFill(new Coord3D(this.Coord.X + 1, this.Coord.Y, this.Coord.Z), Fill.N);
                    case Fill.W:                                     
                        return new CoordFill(new Coord3D(this.Coord.X - 1, this.Coord.Y, this.Coord.Z), Fill.N);
                    case Fill.U:
                        return new CoordFill(new Coord3D(this.Coord.X, this.Coord.Y, this.Coord.Z + 1), Fill.N);
                    case Fill.D:
                        return new CoordFill(new Coord3D(this.Coord.X, this.Coord.Y, this.Coord.Z - 1), Fill.N);
                    default:
                        return new CoordFill(Coord3D.InvalidValue, Fill.N);
                }
            }
        }

        public enum Fill
        {
            N,S,E,W,U,D
        }

        #endregion Methods
    }
}
