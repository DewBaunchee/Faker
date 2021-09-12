using System;
using System.Collections;
using System.Collections.Generic;
using Lib.Strategy.IFace;

namespace Lib.Strategy
{
    public class ListGenerationStrategy : IGenericGenerationStrategy
    {
        private readonly Random _random;

        public ListGenerationStrategy()
        {
            _random = new Random();
        }

        public object Generate()
        {
            return new List<object>();
        }

        public bool IsDefaultValue(object value)
        {
            return value == null;
        }

        public object Generate(Type type, Func<Type, object> factory)
        {
            Type genericArg = type.GetGenericArguments()[0];

            IList list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArg));

            if (list == null)
                throw new Exception("Cannot instantiate such list: " + type.Name);

            int itemCount = _random.Next() % 5;
            for (int i = 0; i < itemCount; i++)
                list.Add(factory.Invoke(genericArg));
            return list;
        }

        public bool CanGenerate(Type type)
        {
            if (type == null) return false;
            return type.GetGenericArguments().Length == 1
                   && NameWithoutGeneric(type.FullName).Equals(NameWithoutGeneric(typeof(List<>).FullName));
        }

        private string NameWithoutGeneric(string name)
        {
            if (name == null)
                return "";
            int length = name.LastIndexOf("`", StringComparison.Ordinal);
            return length == -1 ? name : name.Substring(0, length);
        }
    }
}