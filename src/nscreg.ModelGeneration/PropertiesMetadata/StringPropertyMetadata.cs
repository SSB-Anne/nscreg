﻿namespace nscreg.ModelGeneration.PropertiesMetadata
{
    public class StringPropertyMetadata : PropertyMetadataBase
    {
        public StringPropertyMetadata(
            string name, bool isRequired, string value, string groupName = null, string localizeKey = null)
            : base(name, isRequired, localizeKey, groupName)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override PropertyType Selector => PropertyType.String;
    }
}