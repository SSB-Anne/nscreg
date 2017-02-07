﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using nscreg.Data.Entities;
using nscreg.Data.Infrastructure.EntityConfiguration;

namespace nscreg.Data.Configuration
{
    public class ActivityConfiguration : EntityTypeConfigurationBase<Activity>
    {
        public override void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Unit).WithMany().HasForeignKey(x => x.UnitId);
            SetColumnNames(builder);
        }

        private static void SetColumnNames(EntityTypeBuilder<Activity> builder)
        {
            builder.Property(p => p.Id).HasColumnName("Id");
            builder.Property(p => p.IdDate).HasColumnName("Id_Date");
            builder.Property(p => p.UnitId).HasColumnName("Unit_Id");
            builder.Property(p => p.ActivityRevx).HasColumnName("Activity_Revx");
            builder.Property(p => p.ActivityRevy).HasColumnName("Activity_Revy");
            builder.Property(p => p.ActivityYear).HasColumnName("Activity_Year");
            builder.Property(p => p.ActivityType).HasColumnName("Activity_Type");
            builder.Property(p => p.Employees).HasColumnName("Employees");
            builder.Property(p => p.Turnover).HasColumnName("Turnover");
            builder.Property(p => p.UpdatedBy).HasColumnName("Updated_By");
            builder.Property(p => p.UpdatedDate).HasColumnName("Updated_Date");
        }
    }
}