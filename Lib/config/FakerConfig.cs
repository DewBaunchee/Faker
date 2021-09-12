using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Lib.Strategy.IFace;

namespace Lib.Config
{
    public class FakerConfig
    {
        private readonly Dictionary<PropertyInfo, IGenerationStrategy> _generationStrategies = 
            new Dictionary<PropertyInfo, IGenerationStrategy>();

        public void Add<TSource, TPropType, TGenerator>(Expression<Func<TSource, TPropType>> expression) 
            where TGenerator : IGenerationStrategy
        {
            PropertyInfo propertyInfo = ((MemberExpression) expression.Body).Member as PropertyInfo;
            if(propertyInfo == null)
                throw new Exception("Cannot add config item.");
            _generationStrategies.Add(propertyInfo, Activator.CreateInstance<TGenerator>());
        }

        public IGenerationStrategy GetGenerator(PropertyInfo propertyInfo)
        {
            return _generationStrategies.GetValueOrDefault(propertyInfo, null);
        }
    }
}