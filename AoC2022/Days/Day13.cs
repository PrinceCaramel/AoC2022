using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day13 : IDay
    {
        #region Fields
        //753 too low
        List<Tuple<Packet, Packet>> mPacketPairs = new List<Tuple<Packet, Packet>>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            int lResult = 0;
            foreach (Tuple<Packet, Packet> lPair in this.mPacketPairs)
            {
                if (this.ComparePacket(lPair.Item1, lPair.Item2))
                {
                    Console.WriteLine("RightOrder");
                    lResult += int.Parse(lPair.Item1.Id);
                }
                else
                {
                    Console.WriteLine("WrongOrder");
                }
            }
            return lResult.ToString();
        }

        public string GetSecondPuzzle()
        {
            return "";
        }

        public void ComputesData()
        {
            IEnumerable<string> lData = Utils.GetInputData(this, true).ToList();
            for (int lCounter = 0; lCounter <= lData.Count() / 3; lCounter++)
            {
                Packet lLeft = this.ComputePacket(lData.ElementAt(lCounter * 3), lCounter + 1);
                Packet lRight = this.ComputePacket(lData.ElementAt(lCounter * 3 + 1), lCounter + 1);
                this.mPacketPairs.Add(new Tuple<Packet, Packet>(lLeft, lRight));
            }
        }

        private bool ComparePacket(Packet pLeft, Packet pRight)
        {
            bool lResult = true;
            int lMinCount = Math.Min(pLeft.Children.Count(), pRight.Children.Count());
            for (int lCount = 0; lCount < lMinCount; lCount++)
            {
                if (pLeft.Children[lCount] is PacketInt lIntLeft && pRight.Children[lCount] is PacketInt lIntRight)
                {
                    //Console.WriteLine(string.Format("Compare {0} vs {1}", lIntLeft, lIntRight));
                    if (!lIntLeft.IsEqual(lIntRight))
                    {
                        return lIntRight.IsSuperior(lIntLeft);
                    }
                }
                else if (pLeft.Children[lCount] is Packet lNewLeft && pRight.Children[lCount] is Packet lNewRight)
                {
                    lResult &= this.ComparePacket(lNewLeft, lNewRight);
                }
                else
                {
                    Packet lLeft = pLeft.Children[lCount] as Packet;
                    Packet lRight = pRight.Children[lCount] as Packet;
                    if (lLeft == null)
                    {
                        lLeft = new Packet(null, pLeft.Id);
                        PacketInt lOldLeft = pLeft.Children[lCount] as PacketInt;
                        PacketInt lNewIntLeft = new PacketInt(lLeft, lOldLeft.Id, lOldLeft.Value);
                        lLeft.AddChild(lNewIntLeft);
                    }
                    else
                    {
                        lRight = new Packet(null, pRight.Id);
                        PacketInt lOldRight = pRight.Children[lCount] as PacketInt;
                        PacketInt lNewIntRight = new PacketInt(lRight, lOldRight.Id, lOldRight.Value);
                        lRight.AddChild(lNewIntRight);
                    }
                    lResult &= this.ComparePacket(lLeft, lRight);
                }
            }

            if (pLeft.Children.Count() != pRight.Children.Count())
            {
                lResult &= pLeft.Children.Count() == lMinCount;
            }

            return lResult;
        }

        private Packet ComputePacket(string pLine, int pCounter)
        {
            Packet lCurrentPacket = null;
            char[] lArray = pLine.ToCharArray();
            for (int lCounter = 0; lCounter < lArray.Count(); lCounter++)
            {
                if (lArray[lCounter].Equals('['))
                {
                    Packet lNewPacket = new Packet(null, pCounter.ToString());
                    if (lCurrentPacket == null)
                    {
                        lCurrentPacket = lNewPacket;
                    }
                    else
                    {
                        lCurrentPacket.AddChild(lNewPacket);
                        lCurrentPacket = lNewPacket;
                    }
                }
                else if (lArray[lCounter].Equals(']'))
                {
                    lCurrentPacket = (lCurrentPacket.IsRoot ? lCurrentPacket.Root : lCurrentPacket.Parent) as Packet;
                }
                else if (lArray[lCounter].Equals(','))
                {
                    // Do nothing
                }
                else
                {
                    string lNumber = lArray[lCounter].ToString();
                    if (char.IsDigit(lArray[lCounter + 1]))
                    {
                        lNumber += lArray[lCounter + 1].ToString();
                        lCounter++;
                    }
                    PacketInt lPacket = new PacketInt(null, pCounter.ToString(), int.Parse(lNumber));
                    lCurrentPacket.AddChild(lPacket);
                }
            }
            return lCurrentPacket.Root as Packet;
        }

        #endregion Methods
    }

    public class Packet : ATreeElement
    {
        #region Constructors

        public Packet(Packet pParent, string pId) : base(pParent, pId) { }

        #endregion Constructors

        #region Methods

        protected override Func<string> DisplaybleInformation => () => string.Format(" [{0}]", string.Join(",", this.Children.Select(pChild => pChild.ToString())));
        public override string ToString()
        {
            return this.DisplaybleInformation();
        }
        #endregion Methods
    }

    public class PacketInt : ATreeElement
    {
        #region Properties

        public int Value { get; private set; }

        #endregion Properties

        #region Constructors

        public PacketInt(Packet pParent, string pId, int pValue) : base(pParent, pId)
        {
            this.Value = pValue;
        }

        #endregion Constructors

        #region Methods

        public override void AddChild(ATreeElement pChild)
        {
            // Do nothing.
        }

        protected override Func<string> DisplaybleInformation => () => string.Format("{0}", this.Value);
        public override string ToString()
        {
            return this.DisplaybleInformation();
        }

        public bool IsEqual(PacketInt pPacket)
        {
            return this.Value == pPacket.Value;
        }

        public bool IsSuperior(PacketInt pPacket)
        {
            return this.Value > pPacket.Value;
        }

        #endregion Methods
    }
}
