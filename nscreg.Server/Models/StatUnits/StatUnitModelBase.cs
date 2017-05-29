﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using nscreg.Data.Constants;
using nscreg.Server.Models.Lookup;
using nscreg.Utilities.Enums;

namespace nscreg.Server.Models.StatUnits
{
    public class StatUnitModelBase : IStatUnitM
    {
        public string StatId { get; set; }

        [DataType(DataType.Date)]
        public DateTime StatIdDate { get; set; }

        public int TaxRegId { get; set; }

        [DataType(DataType.Date)]
        public DateTime TaxRegDate { get; set; }

        public int ExternalId { get; set; }
        public int ExternalIdType { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExternalIdDate { get; set; }

        public string DataSource { get; set; }
        public int RefNo { get; set; }

        [Required]
        public string Name { get; set; }

        public string ShortName { get; set; }
        public AddressM Address { get; set; }
        public int PostalAddressId { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string TelephoneNo { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataType(DataType.Url)]
        public string WebAddress { get; set; }

        public int? RegMainActivityId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RegistrationReason { get; set; }

        [DataType(DataType.Date)]
        public string LiqDate { get; set; }

        public string LiqReason { get; set; }
        public string SuspensionStart { get; set; }
        public string SuspensionEnd { get; set; }
        public string ReorgTypeCode { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReorgDate { get; set; }

        public string ReorgReferences { get; set; }
        public AddressM ActualAddress { get; set; }
        public string ContactPerson { get; set; }
        public int Employees { get; set; }
        public int NumOfPeople { get; set; }

        [DataType(DataType.Date)]
        public DateTime EmployeesYear { get; set; }

        [DataType(DataType.Date)]
        public DateTime EmployeesDate { get; set; }

        public decimal Turnover { get; set; }

        [DataType(DataType.Date)]
        public DateTime TurnoverYear { get; set; }

        [DataType(DataType.Date)]
        public DateTime TurnoveDate { get; set; }

        public StatUnitStatuses Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime StatusDate { get; set; }

        public string Notes { get; set; }
        public bool FreeEconZone { get; set; }
        public string ForeignParticipation { get; set; }
        public List<CodeLookupVm> CountryCategory { get; set; }
        public string Classified { get; set; }
        public List<ActivityM> Activities { get; set; }
        public ICollection<string> DataAccess { get; set; }
        public ChangeReasons ChangeReason { get; set; }
        public string EditComment { get; set; }
    }
}
