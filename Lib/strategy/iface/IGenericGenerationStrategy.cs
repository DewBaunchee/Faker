using System;

namespace Lib.Strategy.IFace
{
    public interface IGenericGenerationStrategy : IGenerationStrategy
    {
        object Generate(Type type, Func<Type, object> factory);
        bool CanGenerate(Type type);
    }
}