using AoC2022.Interfaces;
using AoC2022.Utilities;

namespace AoC2022
{
    internal class Program
    {
        #region Fields

        private static int mCurrentDay = 8;

        #endregion Fields

        #region Methods

        private static void Main(string[] pArgs)
        {
            DayChooser lDayChooser = new DayChooser();
            IDay lCurrentDay = lDayChooser.Of(mCurrentDay);
            lCurrentDay.ComputesData();
            Console.WriteLine(string.Format("Puzzle1 : {0}\nPuzzle2 : {1}", lCurrentDay.GetFirstPuzzle(), lCurrentDay.GetSecondPuzzle()));
        }

        #endregion Methods
    }
}