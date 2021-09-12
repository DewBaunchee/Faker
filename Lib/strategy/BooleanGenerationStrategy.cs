using System;
using System.Collections.Generic;
using Lib.Strategy.IFace;

namespace Lib.Strategy
{
    public class BooleanGenerationStrategy : ITypeGenerationStrategy
    {

        private readonly Random _random;

        public BooleanGenerationStrategy()
        {
            _random = new Random();
        }
        
        public object Generate()
        {
            return (_random.Next() & 1) == 1;
        }

        public bool IsDefaultValue(object value)
        {
            return true;
        }

        public List<Type> ValueTypes()
        {
            return new List<Type> { typeof(bool) };
        }
    }
}