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
    }
}
