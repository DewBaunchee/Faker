using System;
using System.Collections.Generic;

namespace Lib.Strategy.IFace
{
    public interface ITypeGenerationStrategy : IGenerationStrategy
    {
        List<Type> ValueTypes();
    }
}