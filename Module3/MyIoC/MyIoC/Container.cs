using MyIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyIoC
{  
    public class Container
    {
        Dictionary <Type, Type> types;   
        public Container()
        {           
            types = new Dictionary<Type, Type>();
        }
       
        public void Register(Type type)
        {
            types.Add(type, type);
        }
        public void Register(Type type, Type concreteType)
        {
            types.Add(type, concreteType);          
        }

        public object CreateInstance(Type type)
        {
            Type dependendType = types[type];
            
            ConstructorInfo constructorInfo = dependendType.GetConstructors().First();
           
            var instance = CreateFromConstructor(dependendType, constructorInfo);

            if (dependendType.GetCustomAttribute<ImportConstructorAttribute>() != null)
               return instance;
            
            CreateProperties(dependendType, instance);
            return instance;
        }
        public T CreateInstance<T>()
        {
            var type = typeof(T);
            return (T)CreateInstance(type);          
        }
        object CreateFromConstructor(Type type, ConstructorInfo constructorInfo)
        {
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            
            List<object> parametersList = new List<object>(parameters.Length);
           
            foreach (ParameterInfo item in parameters)
            {
                parametersList.Add(CreateInstance(item.ParameterType));
            }
           
            return Activator.CreateInstance(type, parametersList.ToArray());          
        }

         void CreateProperties(Type type, object instance)
        {
            var propertiesInfo = type.GetProperties().Where(pr => pr.GetCustomAttribute<ImportAttribute>() != null);
            foreach (var property in propertiesInfo)
            {
                var resolvedProperty = CreateInstance(property.PropertyType);
                property.SetValue(instance, resolvedProperty);
            }     
        }

        public void AddAssembly(Assembly assembly)
        {
            var exportedTypes = assembly.ExportedTypes;

            foreach (var type in exportedTypes)
            {
                var importConstructorAttribute = type.GetCustomAttribute<ImportConstructorAttribute>();

                if (importConstructorAttribute != null)
                {
                    types.Add(type, type);
                }

                var importAttributes = type.GetProperties().Where(pr => pr.GetCustomAttribute<ImportAttribute>() != null);

                if (importAttributes.Count() != 0)
                {
                    types.Add(type, type);
                }

                var exportAttributes = type.GetCustomAttributes<ExportAttribute>();

                foreach (var exportAttribute in exportAttributes)
                {
                    if (exportAttribute.dependentType != null)
                    {
                        types.Add(exportAttribute.dependentType, type);
                    }
                    else
                    {
                        types.Add(type, type);
                    }
                }
            }
        }

    }
}
