﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace nscreg.ModelGeneration
{
    public static class PropertyMetadataFactory
    {
        private static readonly IEnumerable<IPropertyCreator> PropertyCreators;

        static PropertyMetadataFactory()
        {
            PropertyCreators = typeof(PropertyMetadataFactory).GetTypeInfo().Assembly.GetTypes()
                .Where(x => !x.GetTypeInfo().IsAbstract && typeof(IPropertyCreator).IsAssignableFrom(x))
                .Select(Activator.CreateInstance)
                .Cast<IPropertyCreator>();
        }

        public static PropertyMetadataBase Create(PropertyInfo propertyInfo, object obj)
        {
            var creator = PropertyCreators.FirstOrDefault(x => x.CanCreate(propertyInfo));
            if (creator == null)
                throw new ArgumentException(
                    $"can't create property metadata for property {propertyInfo.Name} of type {propertyInfo.PropertyType}");
            return creator.Create(propertyInfo, obj);
        }
    }
}