using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public enum RPS
    {
        Rock = 0,
        Paper = 1,
        Scissor = 2
    }

    public static class RPSExtensions
    {
        public static RPS GetDraw(this RPS pRPS)
        {
            return pRPS;
        }

        public static RPS GetLosingRPS(this RPS pRPS)
        {
            return (RPS)(((int)pRPS + 1) % 3);
        }

        public static RPS GetWinningRPS(this RPS pRPS)
        {
            return (RPS)(((int)pRPS + 2) % 3);
        }
    }

    public class Day2 : IDay
    {
        #region Fields

        List<Tuple<RPS, string>> mBattles = new List<Tuple<RPS, string>>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            return this.GetScore(this.mBattles.Select(pBattle => this.GetRPS1(pBattle))).ToString();
        }

        public string GetSecondPuzzle()
        {
            return this.GetScore(this.mBattles.Select(pBattle => this.GetRPS2(pBattle))).ToString();
        }

        public void ComputesData()
        {
            IEnumerable<string> lInput = Utils.GetInputData(this);
            foreach (string lBattle in lInput)
            {
                string[] lChoices = lBattle.Split(' ');
                this.mBattles.Add(new Tuple<RPS, string>(this.GetFirstPlayerGame(lChoices[0]), lChoices[1]));
            }
        }

        private RPS GetFirstPlayerGame(string pPlay)
        {
            if (pPlay.Equals("A"))
                return RPS.Rock;
            if (pPlay.Equals("B"))
                return RPS.Paper;
            return RPS.Scissor;
        }

        private Tuple<RPS, RPS> GetRPS1(Tuple<RPS, string> pBattle)
        {
            RPS lWhatToPlay = RPS.Scissor;
            if (pBattle.Item2.Equals("X"))
                lWhatToPlay = RPS.Rock;
            if (pBattle.Item2.Equals("Y"))
                lWhatToPlay = RPS.Paper;
            return new Tuple<RPS, RPS>(pBattle.Item1, lWhatToPlay);
        }

        private Tuple<RPS, RPS> GetRPS2(Tuple<RPS, string> pBattle)
        {
            RPS lWhatToPlay = pBattle.Item1.GetDraw();
            if (pBattle.Item2 == "X")
            {
                lWhatToPlay = pBattle.Item1.GetWinningRPS();
            }
            if (pBattle.Item2 == "Z")
            {
                lWhatToPlay = pBattle.Item1.GetLosingRPS();
            }
            return new Tuple<RPS, RPS>(pBattle.Item1, lWhatToPlay);
        }

        private int GetVictoryPoints(Tuple<RPS, RPS> pBattle)
        {
            if (pBattle.Item2 == pBattle.Item1.GetDraw())
                return 3;
            if (pBattle.Item2 == pBattle.Item1.GetWinningRPS())
                return 0;
            return 6;
        }

        private int GetPoint(RPS pValue)
        {
            return (int)pValue + 1;
        }

        private int GetScore(IEnumerable<Tuple<RPS, RPS>> pRounds)
        {
            return pRounds.Select(pBattle => this.GetVictoryPoints(pBattle) + this.GetPoint(pBattle.Item2)).Sum();
        }

        #endregion Methods
    }
}
