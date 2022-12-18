using AoC2022.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Utilities
{
    public static class Utils
    {
        private static string GetFullPath(string pFileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), string.Format(@"..\..\..\Data\{0}.txt", pFileName));
        }

        private static IEnumerable<string> GetInput(string pFileName)
        {
            return File.ReadLines(Utils.GetFullPath(pFileName));
        }

        public static IEnumerable<string> GetInputData(IDay pDay, bool pIsTest = false)
        {
            return GetInput(string.Format("{0}{1}", pDay.GetType().Name, pIsTest ? "Test" : ""));
        }

        public static List<Tuple<int, int>> GetNeighbors(int pCurrentX, int pCurrentY, int pMaxX, int pMaxY, bool pIncludeDiagonals = false)
        {
            List<Tuple<int, int>> lResult = new List<Tuple<int, int>>();
            if (pCurrentX - 1 >= 0)
            {
                lResult.Add(new Tuple<int, int>(pCurrentX - 1, pCurrentY));
            }
            if (pCurrentY - 1 >= 0)
            {
                lResult.Add(new Tuple<int, int>(pCurrentX, pCurrentY - 1));
            }
            if (pCurrentY + 1 <= pMaxY)
            {
                lResult.Add(new Tuple<int, int>(pCurrentX, pCurrentY + 1));
            }
            if (pCurrentX + 1 <= pMaxX)
            {
                lResult.Add(new Tuple<int, int>(pCurrentX + 1, pCurrentY));
            }
            if (pIncludeDiagonals)
            {
                if (pCurrentX - 1 >= 0 && pCurrentY - 1 >= 0)
                {
                    lResult.Add(new Tuple<int, int>(pCurrentX - 1, pCurrentY - 1));
                }
                if (pCurrentX - 1 >= 0 && pCurrentY + 1 <= pMaxY)
                {
                    lResult.Add(new Tuple<int, int>(pCurrentX - 1, pCurrentY + 1));
                }
                if (pCurrentX + 1 <= pMaxX && pCurrentY - 1 >= 0)
                {
                    lResult.Add(new Tuple<int, int>(pCurrentX + 1, pCurrentY - 1));
                }
                if (pCurrentX + 1 <= pMaxX && pCurrentY + 1 <= pMaxY)
                {
                    lResult.Add(new Tuple<int, int>(pCurrentX + 1, pCurrentY + 1));
                }
            }
            return lResult;
        }
    }

    public struct Coord
    {
        public Int64 X;
        public Int64 Y;
        public bool IsValid { get => !this.Equals(Coord.InvalidValue); }
        public Coord(Int64 pX, Int64 pY) { this.X = pX; this.Y = pY; }
        public override string ToString()
        {
            return string.Format("{0},{1}", this.X, this.Y);
        }

        public static Coord operator +(Coord pC1, Coord pC2) => new Coord(pC1.X + pC2.X, pC1.Y + pC2.Y);
        public static Coord operator -(Coord pC1, Coord pC2) => new Coord(pC1.X - pC2.X, pC1.Y - pC2.Y);

        public static Coord Origin { get { return new Coord(0, 0); } }
        public static Coord InvalidValue { get { return new Coord(Int64.MaxValue, Int64.MaxValue); } }

        public List<Coord> GetNeighbors(int pMinX, int pMinY, int pMaxX, int pMaxY)
        {
            List<Coord> lNeighbors = new List<Coord>();
            if (this.X - 1 >= pMinX)
            {
                lNeighbors.Add(new Coord(this.X - 1, this.Y));
            }
            if (this.Y - 1 >= pMinY)
            {
                lNeighbors.Add(new Coord(this.X, this.Y - 1));
            }
            if (this.Y + 1 <= pMaxY)
            {
                lNeighbors.Add(new Coord(this.X, this.Y + 1));
            }
            if (this.X + 1 <= pMaxX)
            {
                lNeighbors.Add(new Coord(this.X + 1, this.Y));
            }
            return lNeighbors;
        }
    }

    public struct Coord3D
    {
        public Int64 X;
        public Int64 Y;
        public Int64 Z;
        public bool IsValid { get => !this.Equals(Coord3D.InvalidValue); }
        public Coord3D(Int64 pX, Int64 pY, Int64 pZ) { this.X = pX; this.Y = pY; this.Z = pZ; }
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", this.X, this.Y, this.Z);
        }

        public static Coord3D operator +(Coord3D pC1, Coord3D pC2) => new Coord3D(pC1.X + pC2.X, pC1.Y + pC2.Y, pC1.Z + pC2.Z);
        public static Coord3D operator -(Coord3D pC1, Coord3D pC2) => new Coord3D(pC1.X - pC2.X, pC1.Y - pC2.Y, pC1.Z - pC2.Z);

        public static Coord3D Origin { get { return new Coord3D(0, 0, 0); } }
        public static Coord3D InvalidValue { get { return new Coord3D(Int64.MaxValue, Int64.MaxValue, Int64.MaxValue); } }

        public List<Coord3D> GetNeighbors(int pMinX, int pMinY, int pMinZ, int pMaxX, int pMaxY, int pMaxZ)
        {
            List<Coord3D> lNeighbors = new List<Coord3D>();
            if (this.X - 1 >= pMinX)
            {
                lNeighbors.Add(new Coord3D(this.X - 1, this.Y, this.Z));
            }
            if (this.Y - 1 >= pMinY)
            {
                lNeighbors.Add(new Coord3D(this.X, this.Y - 1, this.Z));
            }
            if (this.Z - 1 >= pMinZ)
            {
                lNeighbors.Add(new Coord3D(this.X, this.Y, this.Z - 1));
            }
            if (this.Y + 1 <= pMaxY)
            {
                lNeighbors.Add(new Coord3D(this.X, this.Y + 1, this.Z));
            }
            if (this.X + 1 <= pMaxX)
            {
                lNeighbors.Add(new Coord3D(this.X + 1, this.Y, this.Z));
            }
            if (this.Z + 1 <= pMaxZ)
            {
                lNeighbors.Add(new Coord3D(this.X, this.Y, this.Z + 1));
            }
            return lNeighbors;
        }

        public static Coord3D GetFromString(string pString)
        {
            Int64[] lSplit = pString.Split(',').Select(pInt => Int64.Parse(pInt)).ToArray();
            return new Coord3D(lSplit[0], lSplit[1], lSplit[2]);
        }
    }
}
