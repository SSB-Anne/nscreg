﻿using System;
using System.Linq;
using nscreg.Data.Constants;
using nscreg.Data.Entities;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace nscreg.Data
{
    internal static partial class SeedData
    {
        public static void AddDataSources(NSCRegDbContext context)
        {
            var ds1 = new DataSource
            {
                Name = "data source #1",
                Description = "data source #1 detailed description",
                Priority = DataSourcePriority.Ok,
                StatUnitType = (StatUnitTypes) 1,
                Restrictions = null,
                VariablesMapping = "id-RegId,name-Name",
                AttributesToCheck = "id,name,something",
                AllowedOperations = DataSourceAllowedOperation.CreateAndAlter,
            };
            var ds2 = new DataSource
            {
                Name = "data source #2",
                Description = "data source #2 detailed description",
                Priority = DataSourcePriority.Trusted,
                StatUnitType = (StatUnitTypes) 2,
                Restrictions = null,
                VariablesMapping = "id-RegId,whatever-Name",
                AttributesToCheck = "id,salary,whatever",
                AllowedOperations = DataSourceAllowedOperation.Create,
            };

            context.DataSources.AddRange(ds1, ds2);
            context.SaveChanges();

            var dsq1 = new DataSourceQueue
            {
                DataSource = ds1,
                DataSourceFileName = "qwe.xml",
                DataSourcePath = ".\\",
                DataUploadingLogs = new[]
                {
                    new DataUploadingLog
                    {
                        StartImportDate = DateTime.Now,
                        EndImportDate = DateTime.Now,
                        StatUnitName = "qwe",
                        SerializedUnit = JsonConvert.SerializeObject(new LegalUnit())
                    }
                },
                EndImportDate = DateTime.Now,
                StartImportDate = DateTime.Now,
                User = context.Users.FirstOrDefault(),
            };

            context.DataSourceQueues.Add(dsq1);
            context.SaveChanges();
        }
    }
}