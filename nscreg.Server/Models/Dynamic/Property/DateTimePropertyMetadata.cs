﻿using System;

namespace nscreg.Server.Models.Dynamic.Property
{
    public class DateTimePropertyMetadata : PropertyMetadataBase
    {
        public DateTimePropertyMetadata(string name, bool isRequired, DateTime? value) : base(name, isRequired)
        {
            Value = value;
        }

        public DateTime? Value { get; set; }
        public override PropertyType Selector => PropertyType.DateTime;

    }
}