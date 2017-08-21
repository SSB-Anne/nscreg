﻿using System.Collections.Generic;
using System.Linq;
using nscreg.Data.Entities;

namespace nscreg.Business.Analysis.StatUnit.Rules
{
    /// <summary>
    /// Stat unit analysis connection manager
    /// </summary>
    public class ConnectionsManager
    {
        private readonly IStatisticalUnit _unit;

        public ConnectionsManager(IStatisticalUnit unit)
        {
            _unit = unit;
        }

        public (string, string[]) CheckAddress(List<Address> addresses)
        {
            return _unit.Address == null
                ? ("Address", new[] {"Stat unit doesn't have related address"})
                : addresses.Any(
                    a => a.AddressPart1 == _unit.Address.AddressPart1 && a.RegionId == _unit.Address.RegionId)
                    ? (null, null)
                    : ("Address", new[] {"Stat unit doesn't have related address"});
        }
        
    }
}