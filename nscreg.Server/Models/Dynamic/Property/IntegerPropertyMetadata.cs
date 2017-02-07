﻿namespace nscreg.Server.Models.Dynamic.Property
{
    public class IntegerPropertyMetadata : PropertyMetadataBase
    {
        public IntegerPropertyMetadata(string name, bool isRequired, int? value, string localizeKey = null)
            : base(name, isRequired, localizeKey)
        {
            Value = value;
        }

        public int? Value { get; set; }
        public override PropertyType Selector => PropertyType.Integer;
    }
}