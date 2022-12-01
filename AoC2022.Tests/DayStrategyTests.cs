using AoC2022;
using AoC2022.Interfaces;
using AoC2022.Utilities;

namespace AoC2022.Tests
{
    [TestFixture]
    public class DayStrategyTests
    {
        private DayChooser mDayStrategy;

        [SetUp]
        public void Setup()
        {
            this.mDayStrategy = new DayChooser();
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-3)]
        public void Given_negative_day_Returns_null(int pValue)
        {
            IDay lActual = this.mDayStrategy.Of(pValue);
            Assert.That(lActual, Is.EqualTo(null));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Given_0_Or_above_Returns_value(int pValue)
        {
            IDay lDay = this.mDayStrategy.Of(pValue);
            int lActual;
            int.TryParse(lDay.GetType().Name.Remove(0, 3), out lActual);
            Assert.That(lActual, Is.EqualTo(pValue));
        }
    }
}