using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lib.Config;
using Lib.Generator;
using Lib.Strategy.IFace;

namespace Lib
{
    public class Faker
    {
        private readonly Stack<Type> _preInstantiating;
        private readonly Stack<object> _instantiating;
        private readonly ValueGenerator _generator;
        private readonly FakerConfig _config;

        public Faker()
        {
            _preInstantiating = new Stack<Type>();
            _instantiating = new Stack<object>();
            _generator = new ValueGenerator(this);
        }

        public Faker(FakerConfig config) : this()
        {
            _config = config;
        }

        public T Create<T>()
        {
            T instance = (T) Instantiate(typeof(T));

            if (_preInstantiating.Count > 0 || _instantiating.Count > 0)
                throw new Exception("Stacks are not clear.");

            return instance;
        }

        internal object Instantiate(Type type)
        {
            ValidateForInstantiating(type);

            object value = _generator.Generate(type);
            if (value != null)
                return value;

            CheckCyclingDependency(type);

            foreach (var obj in _instantiating)
                if (obj.GetType() == type)
                    return obj;

            object any = Construct(type);
            _instantiating.Push(any);
            InjectFields(any);
            InjectProperties(any);
            _instantiating.Pop();
            return any;
        }

        private void ValidateForInstantiating(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
                throw new Exception("Cannot instantiate this type: " + type.Name);
        }

        private void CheckCyclingDependency(Type type)
        {
            Type prevObjType = null;
            foreach (var objType in _preInstantiating)
            {
                if (objType == type)
                    throw new Exception("Cycling depending: " +
                                        (prevObjType == null ? objType.Name : prevObjType.Name) + " <-> " + type.Name);
                prevObjType = objType;
            }
        }

        private object Construct(Type type)
        {
            List<ConstructorInfo> bestConstructors = type.GetConstructors().ToList();
            bestConstructors.Sort(
                (a, b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length)
            );

            if (bestConstructors.Count == 0)
                return Activator.CreateInstance(type);

            foreach (var constructor in bestConstructors)
            {
                try
                {
                    return constructor.Invoke(
                        GetParameters(constructor)
                    );
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            throw new Exception("Cannot construct instance of type: " + type.Name);
        }

        private void InjectFields(object any)
        {
            FieldInfo[] fields = any.GetType().GetFields().Where(field => field.IsPublic && !field.IsInitOnly)
                .ToArray();

            foreach (var field in fields)
            {
                if (ValueNotInitialized(field.GetValue(any)))
                    field.SetValue(any, Instantiate(field.FieldType));
            }
        }

        private void InjectProperties(object any)
        {
            PropertyInfo[] properties = any.GetType().GetProperties().Where(property => property.CanWrite).ToArray();
            foreach (var property in properties)
            {
                IGenerationStrategy strategy = _config?.GetGenerator(property);
                if (strategy == null)
                {
                    if (ValueNotInitialized(property.GetValue(any)))
                        property.SetValue(any, Instantiate(property.PropertyType));
                }
                else
                {
                    if (strategy.IsDefaultValue(property.GetValue(any)))
                        property.SetValue(any, strategy.Generate());
                }
            }
        }

        private bool ValueNotInitialized(object value)
        {
            return _generator.IsDefaultValue(value);
        }

        private object[] GetParameters(ConstructorInfo constructor)
        {
            _preInstantiating.Push(constructor.DeclaringType);
            object[] parameters = new object[constructor.GetParameters().Length];
            int i = 0;
            foreach (var parameter in constructor.GetParameters())
                parameters[i++] = Instantiate(parameter.ParameterType);
            _preInstantiating.Pop();
            return parameters;
        }
    }
}