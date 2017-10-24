using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using nscreg.Data.Core.EntityConfiguration;
using nscreg.Data.Entities;

namespace nscreg.Data.Configuration
{
    /// <summary>
    ///  Класс конфигурации персоны стат. единицы
    /// </summary>
    public class PersonStatisticalUnitConfiguration : EntityTypeConfigurationBase<PersonStatisticalUnit>
    {
        public override void Configure(EntityTypeBuilder<PersonStatisticalUnit> builder)
        {
            builder.HasKey(v => new { v.UnitId, v.PersonId, v.PersonType });
            builder.HasOne(v => v.Person).WithMany(v => v.PersonsUnits).HasForeignKey(v => v.PersonId);
            builder.HasOne(v => v.Unit).WithMany(v => v.PersonsUnits).HasForeignKey(v => v.UnitId);
            builder.HasOne(v => v.StatUnit).WithMany(v => v.StatisticalUnits).HasForeignKey(v => v.StatUnitId);
            builder.HasOne(v => v.GroupUnit).WithMany(v => v.GroupUnits).HasForeignKey(v => v.GroupUnitId);

            builder.Property(p => p.PersonId).HasColumnName("Person_Id");
            builder.Property(p => p.UnitId).HasColumnName("Unit_Id");
            builder.Property(p => p.StatUnitId).HasColumnName("StatUnit_Id");
            builder.Property(p => p.GroupUnitId).HasColumnName("GroupUnit_Id");
        }
    }
}
