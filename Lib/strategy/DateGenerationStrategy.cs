using System;
using System.Collections.Generic;
using Lib.Strategy.IFace;

namespace Lib.Strategy
{
    public class DateGenerationStrategy : ITypeGenerationStrategy
    {
        public object Generate()
        {
            return DateTime.Now;
        }

        public bool IsDefaultValue(object value)
        {
            return true;
        }

        public List<Type> ValueTypes()
        {
            return new List<Type> {typeof(DateTime)};
        }
    }
}