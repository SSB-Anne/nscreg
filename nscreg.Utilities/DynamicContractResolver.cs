﻿using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using nscreg.Utilities.Extensions;

namespace nscreg.Utilities
{
    public class DynamicContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly Type _type;
        private readonly IEnumerable<string> _allowedPropNames;

        /// <summary>
        /// List of allowed property names
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propNames"></param>
        public DynamicContractResolver(Type type, IEnumerable<string> propNames)
        {
            _type = type;
            _allowedPropNames = propNames.Select(x => x.LowerFirstLetter());
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            prop.ShouldSerialize =
                instance => instance.GetType() != _type || _allowedPropNames.Contains(prop.PropertyName);
            return prop;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            => base.CreateProperties(type, memberSerialization)
                .Where(p => _type != type || _allowedPropNames.Contains(p.PropertyName))
                .ToList();
    }
}
