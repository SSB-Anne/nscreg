﻿using System.Collections.Generic;
using FluentValidation;
using nscreg.Data.Constants;
using nscreg.Server.Models.DataAccess;

namespace nscreg.Server.Models.Roles
{
    public class RoleSubmitM
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<int> AccessToSystemFunctions { get; set; }

        public DataAccessModel StandardDataAccess { get; set; }
    }

    public class RoleSubmitMValidator : AbstractValidator<RoleSubmitM>
    {
        public RoleSubmitMValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.AccessToSystemFunctions).NotNull().NotEmpty();
        }
    }
}
