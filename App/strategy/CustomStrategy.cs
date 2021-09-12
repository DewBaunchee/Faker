using Lib.Strategy.IFace;

namespace App.Strategy
{
    public class CustomStrategy : IGenerationStrategy
    {
        public object Generate()
        {
            return "CUSTOMMMMMMMMMMMMMMMMMMMMM";
        }

        public bool IsDefaultValue(object value)
        {
            return value == null;
        }
    }
}