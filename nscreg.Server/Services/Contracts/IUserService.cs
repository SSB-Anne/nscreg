﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using nscreg.Data.Constants;
using nscreg.Server.Models.Users;

namespace nscreg.Server.Services.Contracts
{
    public interface IUserService
    {
        UserListVm GetAllPaged(UserListFilter filter);
        UserVm GetById(string id);
        Task<SystemFunctions[]> GetSystemFunctionsByUserId(string userId);
        Task<ISet<string>> GetDataAccessAttributes(string userId, StatUnitTypes? type);
        Task SetUserStatus(string id, bool isSuspend);
    }
}