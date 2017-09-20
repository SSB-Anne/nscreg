﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using nscreg.Data;
using nscreg.Data.Entities;
using nscreg.Business.Analysis.StatUnit;
using nscreg.Data.Constants;
using nscreg.Server.Common.Models;
using nscreg.Server.Common.Models.StatUnits;
using nscreg.Server.Common.Services.Contracts;

namespace nscreg.Server.Common.Services.StatUnit
{
    /// <inheritdoc />
    /// <summary>
    /// Stat unit analyzing service
    /// </summary>
    public class AnalyzeService : IStatUnitAnalyzeService
    {
        private readonly NSCRegDbContext _ctx;
        private readonly IStatUnitAnalyzer _analyzer;

        public AnalyzeService(NSCRegDbContext ctx, IStatUnitAnalyzer analyzer)
        {
            _ctx = ctx;
            _analyzer = analyzer;
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="M:nscreg.Services.Analysis.StatUnit.IStatUnitAnalyzeService.AnalyzeStatUnit(nscreg.Data.Entities.IStatisticalUnit)" />
        /// </summary>
        public AnalysisResult AnalyzeStatUnit(IStatisticalUnit unit)
        {
            var addresses = _ctx.Address.Where(adr => adr.Id == unit.AddressId).ToList();
            var potentialDuplicateUnits = GetPotentialDuplicateUnits(unit);

            return _analyzer.CheckAll(unit, HasRelatedLegalUnit(unit), HasRelatedAcitivities(unit), addresses, potentialDuplicateUnits);
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="M:nscreg.Services.Analysis.StatUnit.IStatUnitAnalyzeService.AnalyzeStatUnits" />
        /// </summary>
        public void AnalyzeStatUnits()
        {
            var analysisLog = _ctx.AnalysisLogs.LastOrDefault(al => al.ServerEndPeriod == null);
            if (analysisLog == null) return;

            analysisLog.ServerStartPeriod = DateTime.Now;

            var statUnits = _ctx.StatisticalUnits.Include(x => x.PersonsUnits).Include(x => x.Address)
                .Where(su => su.ParentId == null && su.StartPeriod >= analysisLog.UserStartPeriod &&
                             su.EndPeriod <= analysisLog.UserEndPeriod);

            statUnits = analysisLog.LastAnalyzedUnitType == StatUnitTypes.EnterpriseGroup
                ? Enumerable.Empty<StatisticalUnit>().AsQueryable()
                : analysisLog.LastAnalyzedUnitId == null
                    ? statUnits
                    : statUnits.Where(su => su.RegId >= analysisLog.LastAnalyzedUnitId);
            var units = statUnits.ToList();

            foreach (var unit in units)
            {
                var result = AnalyzeStatUnit(unit);

                foreach (var message in result.Messages)
                {
                    _ctx.AnalysisStatisticalErrors.Add(new StatisticalUnitAnalysisError
                    {
                        AnalysisLogId = analysisLog.Id,
                        StatisticalRegId = unit.RegId,
                        ErrorKey = message.Key,
                        ErrorValue = string.Join(";", message.Value)
                    });
                }
                UpdateAnalysisLog(analysisLog, unit, string.Join(";", result.SummaryMessages));
                _ctx.SaveChanges();
            }

            var enterpriseGroups =
                _ctx.EnterpriseGroups.Include(x => x.Address).Where(eg => eg.ParentId == null &&
                                                  eg.StartPeriod >= analysisLog.UserStartPeriod &&
                                                  eg.EndPeriod <= analysisLog.UserEndPeriod);

            enterpriseGroups = analysisLog.LastAnalyzedUnitType == StatUnitTypes.EnterpriseGroup
                ? analysisLog.LastAnalyzedUnitId == null
                    ? enterpriseGroups
                    : enterpriseGroups.Where(eg => eg.RegId >= analysisLog.LastAnalyzedUnitId)
                : enterpriseGroups;
            var groups = enterpriseGroups.ToList();
            foreach (var unit in groups)
            {
                var result = AnalyzeStatUnit(unit);

                foreach (var message in result.Messages)
                {
                    _ctx.AnalysisGroupErrors.Add(new EnterpriseGroupAnalysisError
                    {
                        AnalysisLogId = analysisLog.Id,
                        GroupRegId = unit.RegId,
                        ErrorKey = message.Key,
                        ErrorValue = string.Join(";", message.Value)
                    });
                }
                UpdateAnalysisLog(analysisLog, unit, string.Join(";", result.SummaryMessages));
                _ctx.SaveChanges();
            }
        }

        public SearchVm<InconsistentRecord> GetInconsistentRecords(PaginatedQueryM model, int analysisLogId)
        {
            var summaryMessages = _ctx.AnalysisLogs.FirstOrDefault(al => al.Id == analysisLogId).SummaryMessages;

            var analyzeGroupErrors = _ctx.AnalysisGroupErrors.Where(ae => ae.AnalysisLogId == analysisLogId)
                .Include(x => x.EnterpriseGroup).ToList().GroupBy(x => x.GroupRegId)
                .Select(g => g.First()).ToList();

            var analyzeStatisticalErrors = _ctx.AnalysisStatisticalErrors.Where(ae => ae.AnalysisLogId == analysisLogId)
                .Include(x => x.StatisticalUnit).ToList().GroupBy(x => x.StatisticalRegId)
                .Select(g => g.First());

            var records = new List<InconsistentRecord>();

            records.AddRange(analyzeGroupErrors.Select(error => new InconsistentRecord(error.GroupRegId,
                error.EnterpriseGroup.UnitType, error.EnterpriseGroup.Name, summaryMessages)));
            records.AddRange(analyzeStatisticalErrors.Select(error => new InconsistentRecord(error.StatisticalRegId,
                error.StatisticalUnit.UnitType, error.StatisticalUnit.Name, summaryMessages)));

            var total = records.Count;
            var skip = model.PageSize * (model.Page - 1);
            var take = model.PageSize;

            var paginatedRecords = records.OrderBy(v => v.Type).ThenBy(v => v.Name)
                .Skip(take >= total ? 0 : skip > total ? skip % total : skip)
                .Take(take)
                .ToList();

            return SearchVm<InconsistentRecord>.Create(paginatedRecords, total);
        }

        private List<StatisticalUnit> GetPotentialDuplicateUnits(IStatisticalUnit unit)
        {
            //TODO search from enterprise groups
            if (unit is EnterpriseGroup) return new List<StatisticalUnit>();

            var statUnit = (StatisticalUnit)unit;

            var statUnitPerson = statUnit.PersonsUnits.FirstOrDefault(pu => pu.PersonType == PersonTypes.Owner);

            var units = _ctx.StatisticalUnits
                .Include(x => x.PersonsUnits)
                .Where(su =>
                    su.UnitType == unit.UnitType && su.RegId != unit.RegId &&
                    ((su.StatId == unit.StatId && su.TaxRegId == unit.TaxRegId) || su.ExternalId == unit.ExternalId ||
                     su.Name == unit.Name ||
                     su.ShortName == statUnit.ShortName ||
                     su.TelephoneNo == statUnit.TelephoneNo ||
                     su.AddressId == unit.AddressId ||
                     su.EmailAddress == statUnit.EmailAddress ||
                     su.ContactPerson == statUnit.ContactPerson ||
                     su.PersonsUnits.FirstOrDefault(pu => pu.PersonType == PersonTypes.Owner) != null && statUnitPerson != null &&
                     su.PersonsUnits.FirstOrDefault(pu => pu.PersonType == PersonTypes.Owner).PersonId == statUnitPerson.PersonId &&
                     su.PersonsUnits.FirstOrDefault(pu => pu.PersonType == PersonTypes.Owner).UnitId == statUnitPerson.UnitId
                     ))
                .ToList();
            
            return units;
        }

        private static void UpdateAnalysisLog(AnalysisLog analysisLog, IStatisticalUnit unit, string summaryMessages)
        {
            analysisLog.ServerEndPeriod = DateTime.Now;
            analysisLog.LastAnalyzedUnitId = unit.RegId;
            analysisLog.LastAnalyzedUnitType = unit.UnitType;
            analysisLog.SummaryMessages = string.Join(";", summaryMessages);
        }

        private static bool HasRelatedLegalUnit(IStatisticalUnit unit)
        {
            switch (unit.UnitType)
            {
                case StatUnitTypes.LocalUnit:
                    return ((LocalUnit)unit).LegalUnitId != null;
                case StatUnitTypes.EnterpriseUnit:
                    return ((EnterpriseUnit) unit).LegalUnits.Any();
                case StatUnitTypes.LegalUnit:
                    return true;
                case StatUnitTypes.EnterpriseGroup:
                    return true;
                default:
                    return false;
            }
        }

        private static bool HasRelatedAcitivities(IStatisticalUnit unit)
        {
            if (unit is EnterpriseGroup || unit is LegalUnit) return true;

            var statUnit = (StatisticalUnit)unit;
            return statUnit.ActivitiesUnits.Any();
        }

    }
}