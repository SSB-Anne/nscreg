using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nscreg.Data.Entities
{
    /// <summary>
    /// Enterprise Group type Entity
    /// </summary>
    public class EnterpriseGroupType : LookupBase
    {
        public List<EnterpriseGroup> EnterpriseGroups { get; set; }
    }
}
