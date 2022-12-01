using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Interfaces
{
    public interface IDay
    {
        #region Methods
        void ComputesData();
        string GetFirstPuzzle();
        string GetSecondPuzzle();

        #endregion Methods
    }
}
