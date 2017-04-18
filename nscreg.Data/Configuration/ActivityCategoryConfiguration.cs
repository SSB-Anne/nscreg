﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using nscreg.Data.Entities;
using nscreg.Data.Infrastructure.EntityConfiguration;

namespace nscreg.Data.Configuration
{
    public class ActivityCategoryConfiguration : EntityTypeConfigurationBase<ActivityCategory>
    {
        public override void Configure(EntityTypeBuilder<ActivityCategory> builder)
        {
            builder.Property(v => v.Code)
                .IsRequired()
                .HasMaxLength(10);
            builder.HasIndex(v => v.Code)
                .IsUnique();
            builder.Property(v => v.Name)
                .IsRequired();
            builder.Property(v => v.Section)
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}