﻿using nscreg.Data;
using nscreg.Data.Constants;
using nscreg.Data.Entities;
using nscreg.Server.Core;
using nscreg.Server.Models.StatisticalUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using nscreg.Server.Models.StatisticalUnit.Edit;
using nscreg.Server.Models.StatisticalUnit.Submit;

namespace nscreg.Server.Services
{
    public class StatUnitService
    {
        private readonly Dictionary<Type, Action<IStatisticalUnit, bool>> _deleteUndeleteActions;
        private readonly NSCRegDbContext _context;

        public StatUnitService(NSCRegDbContext context)
        {
            _context = context;
            _deleteUndeleteActions = new Dictionary<Type, Action<IStatisticalUnit, bool>>
            {
                {typeof(EnterpriseGroup), DeleteUndeleteEnterpriseGroupUnit},
                {typeof(EnterpriseUnit), DeleteUndeleteEnterpriseUnits},
                {typeof(LocalUnit), DeleteUndeleteLocalUnits},
                {typeof(LegalUnit), DeleteUndeleteLegalUnits}
            };
        }

        public IStatisticalUnit GetUnitById(StatUnitTypes unitType, int id)
        {
            IStatisticalUnit unit;
            try
            {
                unit = GetNotDeletedStatisticalUnitById(unitType, id);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
            return unit;
        }

        public void DeleteUndelete(StatUnitTypes unitType, int id, bool toDelete)
        {
            IStatisticalUnit unit;
            try
            {
                unit = GetStatisticalUnitById(unitType, id);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }

            _deleteUndeleteActions[unit.GetType()](unit, toDelete);
            _context.SaveChanges();
        }

        private IStatisticalUnit GetStatisticalUnitById(StatUnitTypes unitType, int id)
        {
            switch (unitType)
            {
                case StatUnitTypes.LegalUnit:
                    return _context.LegalUnits.FirstOrDefault(x => x.RegId == id);
                case StatUnitTypes.LocalUnit:
                    return _context.LocalUnits.FirstOrDefault(x => x.RegId == id);
                case StatUnitTypes.EnterpriseUnit:
                    return _context.EnterpriseUnits.FirstOrDefault(x => x.RegId == id);
                case StatUnitTypes.EnterpriseGroup:
                    return _context.EnterpriseGroups.FirstOrDefault(x => x.RegId == id);
            }

            throw new BadRequestException("Statistical unit doesn't exist");
        }

        private IStatisticalUnit GetNotDeletedStatisticalUnitById(StatUnitTypes unitType, int id)
        {
            switch (unitType)
            {
                case StatUnitTypes.LegalUnit:
                    return _context.LegalUnits.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.RegId == id);
                case StatUnitTypes.LocalUnit:
                    return _context.LocalUnits.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.RegId == id);
                case StatUnitTypes.EnterpriseUnit:
                    return _context.EnterpriseUnits.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.RegId == id);
                case StatUnitTypes.EnterpriseGroup:
                    return _context.EnterpriseGroups.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.RegId == id);
            }

            throw new NotFoundException("Statistical unit doesn't exist");
        }

        private void DeleteUndeleteEnterpriseUnits(IStatisticalUnit statUnit,
            bool toDelete)
        {
            ((EnterpriseUnit)statUnit).IsDeleted = toDelete;
        }

        private void DeleteUndeleteEnterpriseGroupUnit(IStatisticalUnit statUnit,
            bool toDelete)
        {
            ((EnterpriseGroup)statUnit).IsDeleted = toDelete;
        }

        private static void DeleteUndeleteLegalUnits(IStatisticalUnit statUnit, bool toDelete)
        {
            ((LegalUnit)statUnit).IsDeleted = toDelete;
        }

        private static void DeleteUndeleteLocalUnits(IStatisticalUnit statUnit, bool toDelete)
        {
            ((LocalUnit)statUnit).IsDeleted = toDelete;
        }

        private void FillBaseFields(StatisticalUnit unit, StatisticalUnitSubmitM data)
        {
            unit.RegIdDate = DateTime.Now;
            unit.StatId = data.StatId;
            unit.StatIdDate = data.StatIdDate;
            unit.TaxRegId = data.TaxRegId;
            unit.TaxRegDate = data.TaxRegDate;
            unit.ExternalId = data.ExternalId;
            unit.ExternalIdType = data.ExternalIdType;
            unit.ExternalIdDate = data.ExternalIdDate;
            unit.DataSource = data.DataSource;
            unit.RefNo = data.RefNo;
            unit.Name = data.Name;
            unit.ShortName = data.ShortName;
            unit.PostalAddressId = data.PostalAddressId;
            unit.TelephoneNo = data.TelephoneNo;
            unit.EmailAddress = data.EmailAddress;
            unit.WebAddress = data.WebAddress;
            unit.RegMainActivity = data.RegMainActivity;
            unit.RegistrationDate = data.RegistrationDate;
            unit.RegistrationReason = data.RegistrationReason;
            unit.LiqDate = data.LiqDate;
            unit.LiqReason = data.LiqReason;
            unit.SuspensionStart = data.SuspensionStart;
            unit.SuspensionEnd = data.SuspensionEnd;
            unit.ReorgTypeCode = data.ReorgTypeCode;
            unit.ReorgDate = data.ReorgDate;
            unit.ReorgReferences = data.ReorgReferences;
            unit.ContactPerson = data.ContactPerson;
            unit.Employees = data.Employees;
            unit.NumOfPeople = data.NumOfPeople;
            unit.EmployeesYear = data.EmployeesYear;
            unit.EmployeesDate = data.EmployeesDate;
            unit.Turnover = data.Turnover;
            unit.TurnoverYear = data.TurnoverYear;
            unit.TurnoveDate = data.TurnoveDate;
            unit.Status = data.Status;
            unit.StatusDate = data.StatusDate;
            unit.Notes = data.Notes;
            unit.FreeEconZone = data.FreeEconZone;
            unit.ForeignParticipation = data.ForeignParticipation;
            unit.Classified = data.Classified;
            if (!string.IsNullOrEmpty(data.AddressPart1) ||
                !string.IsNullOrEmpty(data.AddressPart2) ||
                !string.IsNullOrEmpty(data.AddressPart3) ||
                !string.IsNullOrEmpty(data.AddressPart4) ||
                !string.IsNullOrEmpty(data.AddressPart5) ||
                !string.IsNullOrEmpty(data.GeographicalCodes) ||
                !string.IsNullOrEmpty(data.GpsCoordinates))
                unit.Address = GetAddress<StatisticalUnit>(data);
            if (!string.IsNullOrEmpty(data.ActualAddressPart1) ||
                !string.IsNullOrEmpty(data.ActualAddressPart2) ||
                !string.IsNullOrEmpty(data.ActualAddressPart3) ||
                !string.IsNullOrEmpty(data.ActualAddressPart4) ||
                !string.IsNullOrEmpty(data.ActualAddressPart5) ||
                !string.IsNullOrEmpty(data.ActualGeographicalCodes) ||
                !string.IsNullOrEmpty(data.ActualGpsCoordinates))
                unit.ActualAddress = GetActualAddress<StatisticalUnit>(data, unit.Address);
        }

        public void CreateLegalUnit(LegalUnitSubmitM data)
        {
            var unit = new LegalUnit()
            {
                EnterpriseRegId = data.EnterpriseRegId,
                EntRegIdDate = DateTime.Now,
                Founders = data.Founders,
                Owner = data.Owner,
                Market = data.Market,
                LegalForm = data.LegalForm,
                InstSectorCode = data.InstSectorCode,
                TotalCapital = data.TotalCapital,
                MunCapitalShare = data.MunCapitalShare,
                StateCapitalShare = data.StateCapitalShare,
                PrivCapitalShare = data.PrivCapitalShare,
                ForeignCapitalShare = data.ForeignCapitalShare,
                ForeignCapitalCurrency = data.ForeignCapitalCurrency,
                ActualMainActivity1 = data.ActualMainActivity1,
                ActualMainActivity2 = data.ActualMainActivity2,
                ActualMainActivityDate = data.ActualMainActivityDate
            };
            FillBaseFields(unit, data);
            _context.LegalUnits.Add(unit);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new BadRequestException("Error while create Legal Unit", e);
            }
        }

        public void CreateLocalUnit(LocalUnitSubmitM data)
        {

            var unit = new LocalUnit()
            {
                LegalUnitId = data.LegalUnitId,
                LegalUnitIdDate = data.LegalUnitIdDate
            };
            FillBaseFields(unit, data);
            _context.LocalUnits.Add(unit);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new BadRequestException("Error while create Local Unit", e);
            }
        }

        public void CreateEnterpriseUnit(EnterpriseUnitSubmitM data)
        {
            var unit = new EnterpriseUnit()
            {
                EntGroupId = data.EntGroupId,
                EntGroupIdDate = data.EntGroupIdDate,
                Commercial = data.Commercial,
                InstSectorCode = data.InstSectorCode,
                TotalCapital = data.TotalCapital,
                MunCapitalShare = data.MunCapitalShare,
                StateCapitalShare = data.StateCapitalShare,
                PrivCapitalShare = data.PrivCapitalShare,
                ForeignCapitalShare = data.ForeignCapitalShare,
                ForeignCapitalCurrency = data.ForeignCapitalCurrency,
                ActualMainActivity1 = data.ActualMainActivity1,
                ActualMainActivity2 = data.ActualMainActivity2,
                ActualMainActivityDate = data.ActualMainActivityDate,
                EntGroupRole = data.EntGroupRole

            };
            FillBaseFields(unit, data);
            _context.EnterpriseUnits.Add(unit);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new BadRequestException("Error while create Enterprise Unit", e);
            }
        }

        public void CreateEnterpriseGroupUnit(EnterpriseGroupSubmitM data)
        {
            var unit = new EnterpriseGroup()
            {
                RegIdDate = DateTime.Now,
                StatId = data.StatId,
                StatIdDate = data.StatIdDate,
                TaxRegId = data.TaxRegId,
                TaxRegDate = data.TaxRegDate,
                ExternalId = data.ExternalId,
                ExternalIdType = data.ExternalIdType,
                ExternalIdDate = data.ExternalIdDate,
                DataSource = data.DataSource,
                Name = data.Name,
                ShortName = data.ShortName,
                PostalAddressId = data.PostalAddressId,
                TelephoneNo = data.TelephoneNo,
                EmailAddress = data.EmailAddress,
                WebAddress = data.WebAddress,
                EntGroupType = data.EntGroupType,
                RegistrationDate = data.RegistrationDate,
                RegistrationReason = data.RegistrationReason,
                LiqDateStart = data.LiqDateStart,
                LiqDateEnd = data.LiqDateEnd,
                LiqReason = data.LiqReason,
                SuspensionStart = data.SuspensionStart,
                SuspensionEnd = data.SuspensionEnd,
                ReorgTypeCode = data.ReorgTypeCode,
                ReorgDate = data.ReorgDate,
                ReorgReferences = data.ReorgReferences,
                ContactPerson = data.ContactPerson,
                Employees = data.Employees,
                EmployeesFte = data.EmployeesFte,
                EmployeesYear = data.EmployeesYear,
                EmployeesDate = data.EmployeesDate,
                Turnover = data.Turnover,
                TurnoverYear = data.TurnoverYear,
                TurnoveDate = data.TurnoveDate,
                Status = data.Status,
                StatusDate = data.StatusDate,
                Notes = data.Notes
            };
            if (!string.IsNullOrEmpty(data.AddressPart1) ||
                !string.IsNullOrEmpty(data.AddressPart2) ||
                !string.IsNullOrEmpty(data.AddressPart3) ||
                !string.IsNullOrEmpty(data.AddressPart4) ||
                !string.IsNullOrEmpty(data.AddressPart5) ||
                !string.IsNullOrEmpty(data.GeographicalCodes) ||
                !string.IsNullOrEmpty(data.GpsCoordinates))
                unit.Address = GetAddress<EnterpriseGroup>(data);
            if (!string.IsNullOrEmpty(data.ActualAddressPart1) ||
                !string.IsNullOrEmpty(data.ActualAddressPart2) ||
                !string.IsNullOrEmpty(data.ActualAddressPart3) ||
                !string.IsNullOrEmpty(data.ActualAddressPart4) ||
                !string.IsNullOrEmpty(data.ActualAddressPart5) ||
                !string.IsNullOrEmpty(data.ActualGeographicalCodes) ||
                !string.IsNullOrEmpty(data.ActualGpsCoordinates))
                unit.ActualAddress = GetActualAddress<EnterpriseGroup>(data, unit.Address);
            _context.EnterpriseGroups.Add(unit);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new BadRequestException("Error while create Enterprise Group", e);
            }
        }

        public Address GetAddress<T>(IStatisticalUnitsM data) where T : class, IStatisticalUnit
        {

            var units = _context.Set<T>().Include(a => a.Address).ToList().Where(u => u.Name == data.Name);
            foreach (var unit in units)
            {
                var unitAddress = unit.Address;
                if (unitAddress == null) continue;
                if ((unitAddress.GpsCoordinates != null && data.GpsCoordinates != null) &&
                    unitAddress.GpsCoordinates.Equals(data.GpsCoordinates))
                {
                    throw new BadRequestException(
                        typeof(T).Name + "Error: Name with same GPS Coordinates already excists", null);
                }
                if (((unitAddress.AddressPart1 != null && data.AddressPart1 != null) &&
                     (unitAddress.AddressPart1.Equals(data.AddressPart1))) &&
                    ((unitAddress.AddressPart2 != null && data.AddressPart2 != null) &&
                     (unitAddress.AddressPart2.Equals(data.AddressPart2))) &&
                    ((unitAddress.AddressPart3 != null && data.AddressPart3 != null) &&
                     (unitAddress.AddressPart3.Equals(data.AddressPart3))) &&
                    ((unitAddress.AddressPart4 != null && data.AddressPart4 != null) &&
                     (unitAddress.AddressPart4.Equals(data.AddressPart4))) &&
                    ((unitAddress.AddressPart5 != null && data.AddressPart5 != null) &&
                     (unitAddress.AddressPart5.Equals(data.AddressPart5))))
                {
                    throw new BadRequestException(
                        typeof(T).Name + "Error: Name with same Address already excists", null);
                }
            }
            var address =
                _context.Address.SingleOrDefault(a
                    => a.AddressPart1 == data.AddressPart1 &&
                       a.AddressPart2 == data.AddressPart2 &&
                       a.AddressPart3 == data.AddressPart3 &&
                       a.AddressPart4 == data.AddressPart4 &&
                       a.AddressPart5 == data.AddressPart5 &&
                       (a.GpsCoordinates == data.GpsCoordinates))
                ?? new Address()
                {
                    AddressPart1 = data.AddressPart1,
                    AddressPart2 = data.AddressPart2,
                    AddressPart3 = data.AddressPart3,
                    AddressPart4 = data.AddressPart4,
                    AddressPart5 = data.AddressPart5,
                    GeographicalCodes = data.GeographicalCodes,
                    GpsCoordinates = data.GpsCoordinates
                };
            return address;
        }

        public Address GetActualAddress<T>(IStatisticalUnitsM data, Address unitAddress) where T : class, IStatisticalUnit
        {

            var units = _context.Set<T>().Include(a => a.ActualAddress).ToList().Where(u => u.Name == data.Name);
            foreach (var unit in units)
            {
                var unitActualAddress = unit.ActualAddress;
                if (unitActualAddress == null) continue;
                if ((unitActualAddress.GpsCoordinates != null && data.ActualGpsCoordinates != null) &&
                    unitActualAddress.GpsCoordinates.Equals(data.ActualGpsCoordinates))
                {
                    throw new BadRequestException(
                        typeof(T).Name + "Error: Name with same (Actual) GPS Coordinates already excists", null);
                }
                if (((unitActualAddress.AddressPart1 != null && data.ActualAddressPart1 != null) &&
                     (unitActualAddress.AddressPart1.Equals(data.ActualAddressPart1))) &&
                    ((unitActualAddress.AddressPart2 != null && data.ActualAddressPart2 != null) &&
                     (unitActualAddress.AddressPart2.Equals(data.ActualAddressPart2))) &&
                    ((unitActualAddress.AddressPart3 != null && data.ActualAddressPart3 != null) &&
                     (unitActualAddress.AddressPart3.Equals(data.ActualAddressPart3))) &&
                    ((unitActualAddress.AddressPart4 != null && data.ActualAddressPart4 != null) &&
                     (unitActualAddress.AddressPart4.Equals(data.ActualAddressPart4))) &&
                    ((unitActualAddress.AddressPart5 != null && data.ActualAddressPart5 != null) &&
                     (unitActualAddress.AddressPart5.Equals(data.ActualAddressPart5))))
                {
                    throw new BadRequestException(
                        typeof(T).Name + "Error: Name with same (Actual) Address already excists", null);
                }
            }
            if (unitAddress?.Id == 0)
            {
                if (unitAddress.AddressPart1.Equals(data.ActualAddressPart1) &&
                    unitAddress.AddressPart2.Equals(data.ActualAddressPart2) &&
                    unitAddress.AddressPart3.Equals(data.ActualAddressPart3) &&
                    unitAddress.AddressPart4.Equals(data.ActualAddressPart4) &&
                    unitAddress.AddressPart5.Equals(data.ActualAddressPart5) &&
                    unitAddress.GeographicalCodes.Equals(data.ActualGeographicalCodes) &&
                    unitAddress.GpsCoordinates.Equals(data.ActualGpsCoordinates)) return unitAddress;
            }
            var address =
                _context.Address.SingleOrDefault(a =>
                    a.AddressPart1 == data.ActualAddressPart1 &&
                    a.AddressPart2 == data.ActualAddressPart2 &&
                    a.AddressPart3 == data.ActualAddressPart3 &&
                    a.AddressPart4 == data.ActualAddressPart4 &&
                    a.AddressPart5 == data.ActualAddressPart5 &&
                    (a.GpsCoordinates == data.ActualGpsCoordinates))
                ?? new Address()
                {
                    AddressPart1 = data.ActualAddressPart1,
                    AddressPart2 = data.ActualAddressPart2,
                    AddressPart3 = data.ActualAddressPart3,
                    AddressPart4 = data.ActualAddressPart4,
                    AddressPart5 = data.ActualAddressPart5,
                    GeographicalCodes = data.ActualGeographicalCodes,
                    GpsCoordinates = data.ActualGpsCoordinates
                };
            return address;
        }

        private void EditBaseFields(StatisticalUnit unit, StatisticalUnitEditM data)
        {
            unit.StatId = data.StatId;
            unit.StatIdDate = data.StatIdDate;
            unit.TaxRegId = data.TaxRegId;
            unit.TaxRegDate = data.TaxRegDate;
            unit.ExternalId = data.ExternalId;
            unit.ExternalIdType = data.ExternalIdType;
            unit.ExternalIdDate = data.ExternalIdDate;
            unit.DataSource = data.DataSource;
            unit.RefNo = data.RefNo;
            unit.Name = data.Name;
            unit.ShortName = data.ShortName;
            unit.PostalAddressId = data.PostalAddressId;
            unit.TelephoneNo = data.TelephoneNo;
            unit.EmailAddress = data.EmailAddress;
            unit.WebAddress = data.WebAddress;
            unit.RegMainActivity = data.RegMainActivity;
            unit.RegistrationDate = data.RegistrationDate;
            unit.RegistrationReason = data.RegistrationReason;
            unit.LiqDate = data.LiqDate;
            unit.LiqReason = data.LiqReason;
            unit.SuspensionStart = data.SuspensionStart;
            unit.SuspensionEnd = data.SuspensionEnd;
            unit.ReorgTypeCode = data.ReorgTypeCode;
            unit.ReorgDate = data.ReorgDate;
            unit.ReorgReferences = data.ReorgReferences;
            unit.ContactPerson = data.ContactPerson;
            unit.Employees = data.Employees;
            unit.NumOfPeople = data.NumOfPeople;
            unit.EmployeesYear = data.EmployeesYear;
            unit.EmployeesDate = data.EmployeesDate;
            unit.Turnover = data.Turnover;
            unit.TurnoverYear = data.TurnoverYear;
            unit.TurnoveDate = data.TurnoveDate;
            unit.Status = data.Status;
            unit.StatusDate = data.StatusDate;
            unit.Notes = data.Notes;
            unit.FreeEconZone = data.FreeEconZone;
            unit.ForeignParticipation = data.ForeignParticipation;
            unit.Classified = data.Classified;
            if (!string.IsNullOrEmpty(data.AddressPart1) ||
                !string.IsNullOrEmpty(data.AddressPart2) ||
                !string.IsNullOrEmpty(data.AddressPart3) ||
                !string.IsNullOrEmpty(data.AddressPart4) ||
                !string.IsNullOrEmpty(data.AddressPart5) ||
                !string.IsNullOrEmpty(data.GeographicalCodes) ||
                !string.IsNullOrEmpty(data.GpsCoordinates))
                unit.Address = GetAddress<StatisticalUnit>(data);
            if (!string.IsNullOrEmpty(data.ActualAddressPart1) ||
                !string.IsNullOrEmpty(data.ActualAddressPart2) ||
                !string.IsNullOrEmpty(data.ActualAddressPart3) ||
                !string.IsNullOrEmpty(data.ActualAddressPart4) ||
                !string.IsNullOrEmpty(data.ActualAddressPart5) ||
                !string.IsNullOrEmpty(data.ActualGeographicalCodes) ||
                !string.IsNullOrEmpty(data.ActualGpsCoordinates))
                unit.ActualAddress = GetActualAddress<StatisticalUnit>(data, unit.Address);
        }
        public void EditLegalUnit(LegalUnitEditM data)
        {
            LegalUnit unit;
            try
            {
                unit = _context.LegalUnits.Include(a => a.Address)
                    .Include(aa => aa.ActualAddress)
                    .Single(x => x.RegId == data.RegId);
            }
            catch (Exception e)
            {
                throw new BadRequestException("Error while fetching LegalUnit info from DataBase", e);
            }
            unit.EnterpriseRegId = data.EnterpriseRegId;
            unit.Founders = data.Founders;
            unit.Owner = data.Owner;
            unit.Market = data.Market;
            unit.LegalForm = data.LegalForm;
            unit.InstSectorCode = data.InstSectorCode;
            unit.TotalCapital = data.TotalCapital;
            unit.MunCapitalShare = data.MunCapitalShare;
            unit.StateCapitalShare = data.StateCapitalShare;
            unit.PrivCapitalShare = data.PrivCapitalShare;
            unit.ForeignCapitalShare = data.ForeignCapitalShare;
            unit.ForeignCapitalCurrency = data.ForeignCapitalCurrency;
            unit.ActualMainActivity1 = data.ActualMainActivity1;
            unit.ActualMainActivity2 = data.ActualMainActivity2;
            unit.ActualMainActivityDate = data.ActualMainActivityDate;
            EditBaseFields(unit, data);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new BadRequestException("Error while update LegalUnit info in DataBase", e);
            }
        }

        public void EditLocalUnit(LocalUnitEditM data)
        {

        }

        public void EditEnterpiseUnit(EnterpriseUnitEditM data)
        {

        }

        public void EditEnterpiseGroup(EnterpriseGroupEditM data)
        {

        }
    }
}
