using System;
using System.Collections.Generic;
using Lib.Strategy.IFace;

namespace Lib.Strategy
{
    public class IntegerNumbersGenerationStrategy : ITypeGenerationStrategy
    {
        private static readonly List<Type> ValueTypeList = new List<Type>
        {
            typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), 
            typeof(int), typeof(uint), typeof(long), typeof(ulong)
        };

        private readonly Random _random;

        public IntegerNumbersGenerationStrategy()
        {
            _random = new Random();
        }
        
        public object Generate()
        {
            return _random.Next();
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