using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Strategy.IFace;

namespace Lib.Generator
{
    internal class ValueGenerator
    {
        private static readonly Dictionary<Type, ITypeGenerationStrategy> TypeGenerationStrategies;
        private static readonly List<IGenericGenerationStrategy> ConditionGenerationStrategies;

        static ValueGenerator()
        {
            TypeGenerationStrategies = new Dictionary<Type, ITypeGenerationStrategy>();
            typeof(ValueGenerator).Assembly
                .GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(ITypeGenerationStrategy)))
                .ToList()
                .ForEach(generationStrategyType =>
                    {
                        var generationStrategy = 
                            (ITypeGenerationStrategy) Activator.CreateInstance(generationStrategyType);
                        generationStrategy?.ValueTypes()
                            .ForEach(valueType => TypeGenerationStrategies.Add(valueType, generationStrategy));
                    }
                );
            ConditionGenerationStrategies = typeof(ValueGenerator).Assembly
                .GetTypes()
                .Where(type => 
                    type.GetInterfaces().Contains(typeof(IGenericGenerationStrategy)))
                .Select(generationStrategyType => 
                    (IGenericGenerationStrategy) Activator.CreateInstance(generationStrategyType))
                .ToList();
        }

        private readonly Faker _faker;

        public ValueGenerator(Faker faker)
        {
            _faker = faker;
        }

        public object Generate(Type type)
        {
            if (type.GetGenericArguments().Length > 0)
                return ConditionGenerationStrategies
                    .Find(strategy => strategy.CanGenerate(type))
                    ?.Generate(type, incomingType => _faker.Instantiate(incomingType));

            return TypeGenerationStrategies
                .GetValueOrDefault(type, null)
                ?.Generate();
        }

        public bool IsDefaultValue(object value)
        {
            if (value == null)
                return true;
            
            Type type = value.GetType();
            IGenerationStrategy requiredStrategy;
            
            if (type.GetGenericArguments().Length > 0)
            {
                requiredStrategy = ConditionGenerationStrategies
                    .Find(strategy => strategy.CanGenerate(type));
            }
            else
            {
                requiredStrategy = TypeGenerationStrategies
                    .GetValueOrDefault(type, null);
            }

            if (requiredStrategy == null)
                return true;

            return requiredStrategy.IsDefaultValue(value);
        }
    }
}