using System;
using System.Collections.Generic;
using Lib.Strategy.IFace;

namespace Lib.Strategy
{
    public class FloatNumbersGenerationStrategy : ITypeGenerationStrategy
    {
        private static readonly List<Type> ValueTypeList = new List<Type>
        {
            typeof(float), typeof(double)
        };

        private readonly Random _random;

        public FloatNumbersGenerationStrategy()
        {
            _random = new Random();
        }
        
        public object Generate()
        {
            return _random.NextDouble();
        }

        public bool IsDefaultValue(object value)
        {
            return "0".Equals(value?.ToString());
        }

        public List<Type> ValueTypes()
        {
            return ValueTypeList;
        }
    }
}