using System;
using System.Collections.Generic;
using Lib.Strategy.IFace;

namespace Lib.Strategy
{
    public class CharGenerationStrategy : ITypeGenerationStrategy
    {

        private readonly Random _random;

        public CharGenerationStrategy()
        {
            _random = new Random();
        }
        
        public object Generate()
        {
            return (char) (_random.Next() % 60 + 65);
        }

        public bool IsDefaultValue(object value)
        {
            return (char) value == '\0';
        }

        public List<Type> ValueTypes()
        {
            return new List<Type> { typeof(char) };
        }
    }
}