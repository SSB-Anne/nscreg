using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nscreg.Data.Entities
{
    /// <summary>
    ///  Класс сущность страна
    /// </summary>
    public class Country : LookupBase
    {
        public string Code { get; set; }
    }
}
