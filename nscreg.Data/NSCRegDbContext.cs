﻿using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using nscreg.Data.Entities;
using nscreg.Data.Extensions;

namespace nscreg.Data
{
    // ReSharper disable once InconsistentNaming
    public class NSCRegDbContext : IdentityDbContext<User, Role, string>
    {
        public NSCRegDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<StatisticalUnit> StatisticalUnits { get; set; }
        public DbSet<LegalUnit> LegalUnits { get; set; }
        public DbSet<EnterpriseUnit> EnterpriseUnits { get; set; }
        public DbSet<LocalUnit> LocalUnits { get; set; }
        public DbSet<EnterpriseGroup> EnterpriseGroups { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.AddEntityTypeConfigurations(GetType().GetTypeInfo().Assembly);
        }
    }
}
