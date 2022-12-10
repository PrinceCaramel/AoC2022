using AoC2022.Interfaces;
using AoC2022.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Days
{
    public class Day10 : IDay
    {
        #region Fields

        List<CPUInstruction> mInstructions = new List<CPUInstruction>();
        int mCurrentCycle = 0;
        List<CPUInstruction> mRunningInstructions = new List<CPUInstruction>();
        int mCurrentXValue = 1;
        List<int> mInterceptionValue = new List<int>();
        int mSignal = 0;
        List<string> mCRTLines = new List<string>();

        # endregion Fields

        #region Methods

        public string GetFirstPuzzle()
        {
            this.InitializeData();
            for (int lCycleCount = 0; lCycleCount < 500; lCycleCount++)
            {
                this.RunCycle();
                if (!this.mInstructions.Any())
                {
                    break;
                }
            }

            return this.mSignal.ToString();
        }

        public string GetSecondPuzzle()
        {
            StringBuilder lStringBuilder = new StringBuilder();
            lStringBuilder.AppendLine();
            this.mCRTLines.ForEach(pLine => lStringBuilder.AppendLine(pLine));
            return lStringBuilder.ToString();
        }

        public void ComputesData()
        {
            string lNoop = "noop";
            this.mInstructions = Utils.GetInputData(this).Select(pLine => pLine.StartsWith(lNoop) ? new CPUInstruction(1, 0) : new CPUInstruction(2, int.Parse(pLine.Remove(0, 5)))).ToList();
        }

        private void InitializeData()
        {
            this.mInterceptionValue.Add(20);
            this.mInterceptionValue.Add(60);
            this.mInterceptionValue.Add(100);
            this.mInterceptionValue.Add(140);
            this.mInterceptionValue.Add(180);
            this.mInterceptionValue.Add(220);
            for (int lCount = 0; lCount < 6; lCount++)
            {
                this.mCRTLines.Add(new string('.', 40));
            }
        }

        private void RunCycle()
        {
            this.mCurrentCycle++;
            if (!this.mRunningInstructions.Any())
            {
                this.mRunningInstructions.Add(this.mInstructions[0]);
                this.mInstructions.RemoveAt(0);
            }
            this.mRunningInstructions.ForEach(pInstruction => pInstruction.RunCycle());
            List<CPUInstruction> lOverInstructions = this.mRunningInstructions.Where(pIns => pIns.IsOver).ToList();
            this.mRunningInstructions.RemoveAll(pIns => pIns.IsOver);
            this.ComputeCRT();
            this.InterceptSignalStrength();
            lOverInstructions.ForEach(pIns => this.mCurrentXValue += pIns.Value);
        }

        private void ComputeCRT()
        {
            int lPixelPosition = (this.mCurrentCycle - 1) % 40;
            if (lPixelPosition == this.mCurrentXValue ||
                lPixelPosition == this.mCurrentXValue - 1 ||
                lPixelPosition == this.mCurrentXValue + 1)
            {
                int lLine = (this.mCurrentCycle - 1) / 40;
                this.mCRTLines[lLine] = this.mCRTLines[lLine].Remove(lPixelPosition, 1).Insert(lPixelPosition, "#");
            }
        }

        private void InterceptSignalStrength()
        {
            if (this.mInterceptionValue.Contains(this.mCurrentCycle))
            {
                this.mSignal += this.mCurrentCycle * this.mCurrentXValue;
            }
        }

        #endregion Methods
    }

    public class CPUInstruction
    {
        public CPUInstruction(int pSteps, int pValue)
        {
            this.Steps = pSteps;
            this.Value = pValue;
        }

        public int Value { get; }
        public int Steps { get; private set; }

        public bool IsOver { get => this.Steps == 0; }

        public void RunCycle()
        {
            this.Steps--;
        }
    }
}
