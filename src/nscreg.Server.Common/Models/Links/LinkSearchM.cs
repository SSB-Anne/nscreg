﻿using System;
using FluentValidation;
using nscreg.Data.Constants;
using nscreg.Resources.Languages;

namespace nscreg.Server.Common.Models.Links
{
    public class LinkSearchM
    {
        public string Wildcard { get; set; }
        public StatUnitTypes? Type { get; set; }
        public string RegionCode { get; set; }
        public DateTime? LastChangeFrom { get; set; }
        public DateTime? LastChangeTo { get; set; }
        public string DataSource { get; set; }
    }

    internal class LinkSearchMValidator : AbstractValidator<LinkSearchM>
    {
        public LinkSearchMValidator()
        {
            RuleFor(x => x.LastChangeFrom)
                .LessThanOrEqualTo(x => x.LastChangeTo)
                .When(x => x.LastChangeFrom.HasValue && x.LastChangeTo.HasValue)
                .WithMessage(nameof(Resource.LastChangeFromError));
            RuleFor(x => x.LastChangeTo)
                .GreaterThanOrEqualTo(x => x.LastChangeFrom)
                .When(x => x.LastChangeFrom.HasValue && x.LastChangeTo.HasValue)
                .WithMessage(nameof(Resource.LastChangeToError));
        }
    }
}