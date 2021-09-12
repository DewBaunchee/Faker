using System;
using System.Collections.Generic;
using Lib.Strategy.IFace;

namespace Lib.Strategy
{
    public class StringGenerationStrategy : ITypeGenerationStrategy
    {
        private readonly Random _random;

        public StringGenerationStrategy()
        {
            _random = new Random();
        }

        public object Generate()
        {
            return RandomString();
        }

        public bool IsDefaultValue(object value)
        {
            return value == null || ((string) value).Length == 0;
        }

        private string RandomString()
        {
            char[] chars = new char[_random.Next() % 20];
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char) (_random.Next() % 60 + 65);
            return new string(chars);
        }

        public List<Type> ValueTypes()
        {
            return new List<Type> {"".GetType()};
        }
    }
}