using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            List<Segment> lSegmentsUnion = this.GetSegmentsAtY(lLine);
            return (lSegmentsUnion.Select(pSegment => pSegment.Length).Sum() - 
                this.GetAllBeacons().Where(pCoord => pCoord.Y == lLine).Count()).ToString();
        }

        public string GetSecondPuzzle()
        {
            Coord lFoundBeacon = Coord.Origin;
            for (Int64  lCount = 0; lCount < 4000000; lCount++)
            {
                List<Segment> lSegmentsUnion = this.GetSegmentsAtY(lCount);
                if (lSegmentsUnion.Count > 1)
                {
                    lSegmentsUnion.OrderBy(pSegment => pSegment.Right.X);
                    lFoundBeacon = new Coord(lSegmentsUnion.First().Right.X + 1, lCount);
                    break;
                }
            }
            return (lFoundBeacon.X * 4000000 + lFoundBeacon.Y).ToString();
        }

        private List<Segment> GetSegmentsAtY(Int64 pY)
        {
            List<Segment> lAllSensorsSegments = new List<Segment>();
            foreach (SensorBeacon lBeacon in this.mData)
            {
                Coord[] lCoords = this.GetLineFromSensorBeaconArea(lBeacon, pY);
                if (lCoords != null)
                {
                    lAllSensorsSegments.Add(new Segment(lCoords[0], lCoords[1]));
                }
            }

            List<Segment> lSegmentsUnion = new List<Segment>();
            foreach (Segment lSegment in lAllSensorsSegments)
            {
                Segment lFinalSegment = lSegment;
                if (lSegmentsUnion.Any(pSegment => this.IsLeftIncludedInRight(lFinalSegment, pSegment)))
                {
                    // Do nothing.
                }
                else
                {
                    lSegmentsUnion.RemoveAll(pSegment => this.IsLeftIncludedInRight(pSegment, lFinalSegment));
                    while (lSegmentsUnion.Any(pSegment => this.Intersect(pSegment, lFinalSegment)))
                    {
                        Segment lIntersectingSegment = lSegmentsUnion.First(pSegment => this.Intersect(pSegment, lFinalSegment));
                        lSegmentsUnion.Remove(lIntersectingSegment);
                        lFinalSegment = this.GetUnion(lIntersectingSegment, lFinalSegment);
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

        private Coord[] GetLineFromSensorBeaconArea(SensorBeacon pSensorBeacon, Int64 pY)
        {
            if (pY < pSensorBeacon.NorthPoint.Y || pY > pSensorBeacon.SouthPoint.Y)
            {
                return null;
            }
            Coord[] lResult = new Coord[2];
            Int64  lDiff = Math.Abs(pSensorBeacon.WestPoint.Y - pY);
            Coord lLeft = new Coord(pSensorBeacon.WestPoint.X + lDiff, pY);
            Coord lRight = new Coord(pSensorBeacon.EastPoint.X - lDiff, pY);
            lResult[0] = lLeft;
            lResult[1] = lRight;
            return lResult;
        }

        private bool IsLeftIncludedInRight(Segment pSegment1, Segment pSegment2)
        {
            return (pSegment1.Left.X >= pSegment2.Left.X &&
                pSegment1.Right.X <= pSegment2.Right.X);
        }

        private bool Intersect(Segment pSegment1, Segment pSegment2)
        {
            return (pSegment1.Right.X >= pSegment2.Left.X && pSegment1.Left.X <= pSegment2.Right.X) ||
                (pSegment2.Right.X >= pSegment1.Left.X && pSegment2.Left.X <= pSegment1.Right.X);
        }

        private Segment GetUnion(Segment pSegment1, Segment pSegment2)
        {
            Int64  lY = pSegment1.Left.Y;
            return new Segment(
                new Coord(Math.Min(pSegment1.Left.X, pSegment2.Left.X), lY), 
                new Coord(Math.Max(pSegment1.Right.X, pSegment2.Right.X), lY));
        }

        private IEnumerable<Coord> GetAllBeacons()
        {
            return this.mData.Select(pSB => pSB.ClosestBeacon).Distinct();
        }

        #endregion Methods

        public class SensorBeacon
        {
            public Coord Sensor { get; private set; }
            public Coord ClosestBeacon { get; private set; }

            public Coord NorthPoint  { get; private set; }
            public Coord WestPoint  { get; private set; }
            public Coord EastPoint  { get; private set; }
            public Coord SouthPoint  { get; private set; }

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
                this.WestPoint  = new Coord(lRectBL.X - (Math.Abs((lRectBL - lRectTL).Y) / 2), this.Sensor.Y);
                this.EastPoint  = new Coord(lRectBR.X + (Math.Abs((lRectBR - lRectTR).Y) / 2), this.Sensor.Y);
                this.SouthPoint  = new Coord(this.Sensor.X, lRectTL.Y + (Math.Abs((lRectTL - lRectTR).X) / 2));
                this.NorthPoint  = new Coord(this.Sensor.X, lRectBL.Y - (Math.Abs((lRectBL - lRectBR).X) / 2));
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

            public Int64  Length
            {
                get { return Math.Abs(this.Right.X - this.Left.X) + 1; }
            }

            public Segment(Coord pLeft, Coord pRight)
            {
                this.Left = pLeft;
                this.Right = pRight;
            }

            public override string ToString()
            {
                return string.Format("y:{0} [{1};{2}]", this.Left.Y, this.Left.X, this.Right.X);
            }
        }

        public struct Coord
        {
            public Int64  X;
            public Int64  Y;
            public Coord(Int64  pX, Int64  pY) { this. X = pX; this. Y = pY; }
            public override string ToString()
            {
                return string.Format("{0},{1}", this.X, this.Y);
            }

            public static Coord operator +(Coord pC1, Coord pC2) => new Coord(pC1.X + pC2.X, pC1.Y + pC2.Y);
            public static Coord operator -(Coord pC1, Coord pC2) => new Coord(pC1.X - pC2.X, pC1.Y - pC2.Y);

            public static Coord Origin { get { return new Coord(0, 0); } }
        }
    }
}
