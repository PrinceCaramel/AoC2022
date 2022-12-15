using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static AoC2022.Days.Day15;

namespace AoC2022.Days
{
    public class Day15: IDay
    {
        #region Fields

        List<SensorBeacon> mData = new List<SensorBeacon>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            Int64 lLine = 2000000;
            List<Segment> lSegmentsUnion = this.GetAllSegmentsAtLine(lLine);
            return (lSegmentsUnion.Select(pSegment => pSegment.Length).Sum() - 
                this.GetAllBeacons().Where(pCoord => pCoord.Y == lLine).Count()).ToString();
        }

        public string GetSecondPuzzle()
        {
            Coord lFoundBeacon = Coord.Origin;
            for (Int64  lCount = 0; lCount < 4000000; lCount++)
            {
                List<Segment> lSegmentsUnion = this.GetAllSegmentsAtLine(lCount);
                if (lSegmentsUnion.Count > 1)
                {
                    lSegmentsUnion.OrderBy(pSegment => pSegment.Right.X);
                    lFoundBeacon = new Coord(lSegmentsUnion.First().Right.X + 1, lCount);
                    break;
                }
            }
            return (lFoundBeacon.X * 4000000 + lFoundBeacon.Y).ToString();
        }

        private List<Segment> GetAllSegmentsAtLine(Int64 pY)
        {
            List<Segment> lAllSensorsSegments = new List<Segment>();
            foreach (SensorBeacon lBeacon in this.mData)
            {
                Segment lSegment = lBeacon.GetSegmentAtLine(pY);
                if (lSegment.IsValid)
                {
                    lAllSensorsSegments.Add(lSegment);
                }
            }
            return this.ReduceSegmentsList(lAllSensorsSegments);
        }

        private List<Segment> ReduceSegmentsList(List<Segment> pAllSegments)
        {
            List<Segment> lSegmentsUnion = new List<Segment>();
            foreach (Segment lSegment in pAllSegments)
            {
                Segment lFinalSegment = lSegment;
                if (lSegmentsUnion.Any(pSegment => lFinalSegment.IsIncludedIn(pSegment)))
                {
                    // Do nothing.
                }
                else
                {
                    lSegmentsUnion.RemoveAll(pSegment => pSegment.IsIncludedIn(lFinalSegment));
                    while (lSegmentsUnion.Any(pSegment => pSegment.Intersect(lFinalSegment)))
                    {
                        Segment lIntersectingSegment = lSegmentsUnion.First(pSegment => pSegment.Intersect(lFinalSegment));
                        lSegmentsUnion.Remove(lIntersectingSegment);
                        lFinalSegment = lIntersectingSegment.GetUnionWith(lFinalSegment);
                    }
                    lSegmentsUnion.Add(lFinalSegment);
                }
            }
            return lSegmentsUnion;
        }

        public void ComputesData()
        {
            IEnumerable<string> lData = Utils.GetInputData(this).ToList();
            foreach (string lLine in lData)
            {
                this.mData.Add(this.ComputesSensorBeaconLine(lLine));
            }
        }

        private SensorBeacon ComputesSensorBeaconLine(string pLine)
        {
            string[] lLineSplit = pLine.Replace("Sensor at x=", "").Replace(": closest beacon is at x=", ",").Replace(" ", "").Replace("y=", "").Split(',');
            return new SensorBeacon(new Coord(Int64.Parse(lLineSplit[0]), Int64.Parse(lLineSplit[1])), new Coord(Int64.Parse(lLineSplit[2]), Int64.Parse(lLineSplit[3])));
        }

        private IEnumerable<Coord> GetAllBeacons()
        {
            return this.mData.Select(pSB => pSB.ClosestBeacon).Distinct();
        }

        #endregion Methods

        public class SensorBeacon
        {
            private Coord mNorthPoint;
            private Coord mWestPoint;
            private Coord mEastPoint;
            private Coord mSouthPoint;

            public Coord Sensor { get; private set; }
            public Coord ClosestBeacon { get; private set; }

            public SensorBeacon(Coord pSensor, Coord pClosestBeacon) 
            { 
                this.Sensor = pSensor; 
                this.ClosestBeacon = pClosestBeacon;
                this.ComputeArea();
            }

            private void ComputeArea()
            {
                Coord lDiff = this.ClosestBeacon - this.Sensor;
                Coord lURVector = new Coord(Math.Abs(lDiff.X), Math.Abs(lDiff.Y));
                Coord lDLVector = Coord.Origin - lURVector;
                Coord lULVector = new Coord(-lURVector.X, lURVector.Y);
                Coord lDRVector = Coord.Origin - lULVector;
                Coord lRectBL = this.Sensor + lDLVector;
                Coord lRectBR = this.Sensor + lDRVector;
                Coord lRectTL = this.Sensor + lULVector;
                Coord lRectTR = this.Sensor + lURVector;
                this.mWestPoint  = new Coord(lRectBL.X - (Math.Abs((lRectBL - lRectTL).Y) / 2), this.Sensor.Y);
                this.mEastPoint  = new Coord(lRectBR.X + (Math.Abs((lRectBR - lRectTR).Y) / 2), this.Sensor.Y);
                this.mSouthPoint  = new Coord(this.Sensor.X, lRectTL.Y + (Math.Abs((lRectTL - lRectTR).X) / 2));
                this.mNorthPoint  = new Coord(this.Sensor.X, lRectBL.Y - (Math.Abs((lRectBL - lRectBR).X) / 2));
            }

            public Segment GetSegmentAtLine(Int64 pLine)
            {
                if (pLine < this.mNorthPoint.Y || pLine > this.mSouthPoint.Y)
                {
                    return Segment.InvalidValue;
                }
                Int64 lDiff = Math.Abs(this.mWestPoint.Y - pLine);
                Coord lLeft = new Coord(this.mWestPoint.X + lDiff, pLine);
                Coord lRight = new Coord(this.mEastPoint.X - lDiff, pLine);
                return new Segment(lLeft, lRight);
            }

            public override string ToString()
            {
                return string.Format("Sensor: {0} Beacon: {1}", this.Sensor, this.ClosestBeacon);
            }
        }

        public struct Segment
        {
            public Coord Left;
            public Coord Right;

            public Int64 Length
            {
                get { return Math.Abs(this.Right.X - this.Left.X) + 1; }
            }
            public bool IsValid { get => this.Left.IsValid && this.Right.IsValid; }

            public Segment(Coord pLeft, Coord pRight)
            {
                this.Left = pLeft;
                this.Right = pRight;
            }

            public bool IsIncludedIn(Segment pSegment)
            {
                return (this.Left.X >= pSegment.Left.X &&
                    this.Right.X <= pSegment.Right.X);
            }

            public bool Intersect(Segment pSegment)
            {
                return (this.Right.X >= pSegment.Left.X && this.Left.X <= pSegment.Right.X) ||
                    (pSegment.Right.X >= this.Left.X && pSegment.Left.X <= this.Right.X);
            }

            public Segment GetUnionWith(Segment pSegment)
            {
                Int64 lY = this.Left.Y;
                return new Segment(
                    new Coord(Math.Min(this.Left.X, pSegment.Left.X), lY),
                    new Coord(Math.Max(this.Right.X, pSegment.Right.X), lY));
            }

            public static Segment InvalidValue { get { return new Segment(Coord.InvalidValue, Coord.InvalidValue); } }

            public override string ToString()
            {
                return string.Format("y:{0} [{1};{2}]", this.Left.Y, this.Left.X, this.Right.X);
            }
        }
    }
}
