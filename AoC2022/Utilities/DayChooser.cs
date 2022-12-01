using AoC2022.Interfaces;
using System.Reflection;

namespace AoC2022.Utilities
{
    public class DayChooser
    {
        #region Constructors

        public DayChooser() 
        {
        }

        #endregion Constructors

        #region Methods

        public IDay Of(int pDay)
        {
            IDay lInstance = null;
            Type lFoundType = this.GetDayTypes().Where(pType => this.GetDayFromType(pType).Equals(pDay.ToString())).FirstOrDefault();
            if (lFoundType != null)
            {
                ConstructorInfo lConstructor = lFoundType.GetConstructors().First();
                lInstance = lConstructor.Invoke(new object[] { }) as IDay;
            }
            return lInstance;
        }

        private IEnumerable<Type> GetDayTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(pAssembly => pAssembly.GetTypes())
                .Where(pType => typeof(IDay)
                .IsAssignableFrom(pType));
        }

        private string GetDayFromType(Type pType)
        {
            string lResult = "NaN";
            int lIntResult;
            if (int.TryParse(pType.Name.Remove(0, 3), out lIntResult))
            {
                lResult = lIntResult.ToString();
            }
            return lResult;
        }

        #endregion Methods
    }
}