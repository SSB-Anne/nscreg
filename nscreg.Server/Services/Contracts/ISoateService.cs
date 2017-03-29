﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using nscreg.Data.Entities;
using nscreg.Server.Models.Soates;

namespace nscreg.Server.Services.Contracts
{
    interface ISoateService
    {
        Task<IList<SoateModel>> GetAsync(Expression<Func<Soate, bool>> predicate = null, int limit = 5);

        Task<SoateModel> GetByCode(string soateCode);
    }
}
