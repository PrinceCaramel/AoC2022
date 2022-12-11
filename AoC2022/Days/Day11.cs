using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day11 : IDay
    {
        #region Fields

        List<Monkey> mMonkeys= new List<Monkey>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            Func<UInt64, UInt64> pDecreaseFunction = pVal => (UInt64)(pVal / 3);
            return this.ComputePuzzle(20, pDecreaseFunction);
        }

        public string GetSecondPuzzle()
        {
            this.ComputesData();
            UInt64 lDivisors = (UInt64)this.mMonkeys.Select(pMonkey => pMonkey.DivisibilityValue).Aggregate((pX, pY) => pX * pY);
            Func<UInt64, UInt64> pDecreaseFunction = pVal => (UInt64)(pVal % lDivisors);
            return this.ComputePuzzle(10000, pDecreaseFunction);
        }

        private string ComputePuzzle(int pRounds, Func<UInt64, UInt64> pDecreasingFunction)
        {
            for (int lCount = 0; lCount < pRounds; lCount++)
            {
                foreach (Monkey lMonkey in this.mMonkeys)
                {
                    lMonkey.RunMonkeyActions(this.mMonkeys.ToList(), pDecreasingFunction);
                }
            }
            List<UInt64> lSortedMonkeys = this.mMonkeys.Select(pMonkey => pMonkey.InspectedItems).ToList();
            lSortedMonkeys.Sort();
            lSortedMonkeys.Reverse();
            return (lSortedMonkeys[0] * lSortedMonkeys[1]).ToString();
        }

        public void ComputesData()
        {
            this.mMonkeys.Clear();
            IEnumerable<string> lInput = Utils.GetInputData(this).ToList();
            for (int lIndex = 0; lIndex <= lInput.Count() / 7; lIndex++)
            {
                int lID = this.GetId(lInput.ElementAt(lIndex * 7 + 0));
                IEnumerable<UInt64> lStartingItems = this.GetStartingItems(lInput.ElementAt(lIndex * 7 + 1));
                Func<UInt64, UInt64> lOperation = this.GetOperation(lInput.ElementAt(lIndex * 7 + 2));
                int lDivisor = this.GetDivisor(lInput.ElementAt(lIndex * 7 + 3));
                int lMonkeyTrue = this.GetMonkeyTrue(lInput.ElementAt(lIndex * 7 + 4));
                int lMonkeyFalse = this.GetMonkeyFalse(lInput.ElementAt(lIndex * 7 + 5));
                Monkey lNewMonkey = new Monkey(lID, lStartingItems, lOperation, lDivisor, lMonkeyTrue, lMonkeyFalse);
                this.mMonkeys.Add(lNewMonkey);
            }
        }

        private int GetId(string pLine)
        {
            string lLine = pLine.Remove(pLine.Length - 1, 1);
            lLine = lLine.Remove(0, 7);
            return int.Parse(lLine);
        }

        private IEnumerable<UInt64> GetStartingItems(string pLine)
        {
            string lLine = pLine.Remove(0, 18);
            return lLine.Split(',', ' ', StringSplitOptions.RemoveEmptyEntries).Select(pItem => UInt64.Parse(pItem));
        }

        private Func<UInt64, UInt64> GetOperation(string pLine)
        {
            string lLine = pLine.Remove(0, 23);
            if (lLine[0] == '+')
            {
                UInt64 lValue = UInt64.Parse(lLine.Remove(0, 2));
                return pVal => pVal + lValue;
            }
            else
            {
                lLine = lLine.Remove(0, 2);
                UInt64 lValue;
                if (UInt64.TryParse(lLine, out lValue))
                {
                    return pVal => pVal * lValue;
                }
                else
                {
                    return pVal => pVal * pVal;
                }
            }
        }

        private int GetDivisor(string pLine)
        {
            string lLine = pLine.Remove(0, 21);
            return int.Parse(lLine);
        }

        private int GetMonkeyTrue(string pLine)
        {
            string lLine = pLine.Remove(0, 29);
            return int.Parse(lLine);
        }

        private int GetMonkeyFalse(string pLine)
        {
            string lLine = pLine.Remove(0, 30);
            return int.Parse(lLine);
        }

        #endregion Methods
    }

    public class Monkey
    {
        #region Properties

        public int Id { get; private set; }
        public List<UInt64> Items { get; private set; }
        public Func<UInt64, UInt64> Operation { get; private set; }
        public int DivisibilityValue { get; private set; }
        public UInt64 InspectedItems { get; private set; }
        public int TrueMonkeyId { get; private set; }
        public int FalseMonkeyId { get; private set; }

        #endregion Properties

        #region Constructors

        public Monkey(int pId, IEnumerable<UInt64> pStartingItems, Func<UInt64, UInt64> pOperation,
            int pDivisibilityValue, int pTrueMonkeyId, int pFalseMonkeyId)
        {
            this.InspectedItems = 0;
            this.Id = pId;
            this.Items = new List<UInt64>(); this.Items.AddRange(pStartingItems);
            this.Operation = pOperation;
            this.DivisibilityValue = pDivisibilityValue;
            this.TrueMonkeyId= pTrueMonkeyId;
            this.FalseMonkeyId= pFalseMonkeyId;
        }

        #endregion Constructors

        #region Methods

        public void ReceiveItem(UInt64 pItem)
        {
            this.Items.Add(pItem);
        }

        public void RunMonkeyActions(List<Monkey> pMonkeys, Func<UInt64, UInt64> pDecreaseFunction)
        {
            foreach (UInt64 lItem in this.Items) 
            {
                UInt64 lNewValue = this.Operation(lItem);
                lNewValue = pDecreaseFunction(lNewValue);
                pMonkeys.First(pMonkey => pMonkey.Id == ((lNewValue % (UInt64)this.DivisibilityValue == 0) ? this.TrueMonkeyId : this.FalseMonkeyId)).ReceiveItem(lNewValue);
                this.InspectedItems = this.InspectedItems + 1;
            }
            this.Items.Clear();
        }

        public override string ToString()
        {
            return string.Format("Monkey{0} : Inspected {1}", this.Id, this.InspectedItems);
        }

        #endregion Methods
    }
}
