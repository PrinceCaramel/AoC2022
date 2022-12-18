using AoC2022.Interfaces;
using AoC2022.Utilities;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace AoC2022
{
    internal class Program
    {
        #region Fields

        private static int mCurrentDay = 17;
        private static bool mShouldTimeStamp = true;

        #endregion Fields

        #region Methods

        private static void Main(string[] pArgs)
        {
            DayChooser lDayChooser = new DayChooser();
            IDay lCurrentDay = lDayChooser.Of(mCurrentDay);
            ComputesData(lCurrentDay);
            Console.WriteLine(string.Format("Puzzle1 : {0}\nPuzzle2 : {1}", GetFirstPuzzle(lCurrentDay), GetSecondPuzzle(lCurrentDay)));
        }
        
        private static void ComputesData(IDay pDay)
        {
            if (mShouldTimeStamp)
            {
                Stopwatch lComputingStopWatch = new Stopwatch();
                lComputingStopWatch.Start();
                pDay.ComputesData();
                lComputingStopWatch.Stop();
                Console.WriteLine(string.Format("Computing data: {0}ms", lComputingStopWatch.ElapsedMilliseconds));
            }
            else
            {
                pDay.ComputesData();
            }
        }

        private static string GetFirstPuzzle(IDay pDay)
        {
            string lResult = "";
            if (mShouldTimeStamp)
            {
                Stopwatch lComputingStopWatch = new Stopwatch();
                lComputingStopWatch.Start();
                lResult = pDay.GetFirstPuzzle();
                lComputingStopWatch.Stop();
                lResult = string.Format("{0} (in {1}ms)", lResult, lComputingStopWatch.ElapsedMilliseconds);
            }
            else
            {
                lResult = pDay.GetFirstPuzzle();
            }
            return lResult;
        }

        private static string GetSecondPuzzle(IDay pDay)
        {
            string lResult = "";
            if (mShouldTimeStamp)
            {
                Stopwatch lComputingStopWatch = new Stopwatch();
                lComputingStopWatch.Start();
                lResult = pDay.GetSecondPuzzle();
                lComputingStopWatch.Stop();
                lResult = string.Format("{0} (in {1}ms)", lResult, lComputingStopWatch.ElapsedMilliseconds);
            }
            else
            {
                lResult = pDay.GetSecondPuzzle();
            }
            return lResult;
        }

        #endregion Methods
    }
}