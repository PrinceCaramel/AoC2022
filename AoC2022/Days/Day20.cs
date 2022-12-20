using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day20 : IDay
    {
        #region Fields

        List<CryptedInput> mCode = new List<CryptedInput>();
        CryptedInput mZeroInput;
        int mInputCount;

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            List<CryptedInput> lData = this.Mix(this.mCode);
            int l0Index = lData.IndexOf(this.mZeroInput);
            Int64 l1000thValue = lData.ElementAt((l0Index + 1000) % this.mInputCount).Value;
            Int64 l2000thValue = lData.ElementAt((l0Index + 2000) % this.mInputCount).Value;
            Int64 l3000thValue = lData.ElementAt((l0Index + 3000) % this.mInputCount).Value;
            return (l1000thValue + l2000thValue + l3000thValue).ToString();
        }

        public string GetSecondPuzzle()
        {
            List<CryptedInput> lNewCode = this.mCode.Select(pC => new CryptedInput(pC.Id, pC.Value * 811589153)).ToList();
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            lNewCode = this.Mix(lNewCode);
            int l0Index = lNewCode.IndexOf(this.mZeroInput);
            Int64 l1000thValue = lNewCode.ElementAt((l0Index + 1000) % this.mInputCount).Value;
            Int64 l2000thValue = lNewCode.ElementAt((l0Index + 2000) % this.mInputCount).Value;
            Int64 l3000thValue = lNewCode.ElementAt((l0Index + 3000) % this.mInputCount).Value;
            return (l1000thValue + l2000thValue + l3000thValue).ToString();
        }

        private List<CryptedInput> Mix(List<CryptedInput> pListToMix)
        {
            List<CryptedInput> lListToMix = pListToMix.ToList();
            for (int lCounter = 0; lCounter < this.mInputCount; lCounter++)
            {
                CryptedInput lCode = lListToMix.First(pC => pC.Id == lCounter);
                int lCodeIndex = lListToMix.IndexOf(lCode);
                lListToMix.RemoveAt(lCodeIndex);
                int lNewIndex = (int)((lCodeIndex + lCode.Value) % (this.mInputCount - 1));
                if (lNewIndex < 0)
                {
                    lNewIndex += (this.mInputCount - 1);
                }
                lListToMix.Insert(lNewIndex, lCode);
            }
            return lListToMix;
        }

        public void ComputesData()
        {
            int lId = 0;
            foreach (string lLine in Utils.GetInputData(this))
            {
                int lValue = int.Parse(lLine);
                CryptedInput lCode = new CryptedInput(lId, lValue);
                this.mCode.Add(lCode);
                lId++;
                if (lValue == 0)
                {
                    this.mZeroInput = lCode;
                }
            }
            this.mInputCount = lId;
        }

        #endregion Methods

        public struct CryptedInput
        {
            public int Id { get; private set; }
            public Int64 Value { get; private set; }

            public CryptedInput(int pId, Int64 pValue)
            {
                this.Id = pId;
                this.Value = pValue;
            }
            public override string ToString()
            {
                return string.Format("Value: {0} (id:{1})", this.Value, this.Id);
            }
        }
    }
}
