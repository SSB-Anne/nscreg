﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nscreg.Resources.Languages;
using nscreg.Server.Core;
using nscreg.Server.Models.Lookup;

namespace nscreg.Server.Services
{
    public class CodeLookupProvider<T> where T: CodeLookupVm
    {
        private readonly string _referenceName;
        private readonly Dictionary<string, T> _lookup;

        public CodeLookupProvider(string referenceName, IEnumerable<T> provider)
        {
            _referenceName = referenceName;
            _lookup = provider.ToDictionary(v => v.Code);
        }

        public T Get(string code)
        {
            T data;
            if (_lookup.TryGetValue(code, out data))
            {
                return data;
            }
            throw new BadRequestException(nameof(Resource.CodeLookupFailed), null, _referenceName, code);
        }
    }
}
