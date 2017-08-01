﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using nscreg.Data;
using nscreg.Data.Constants;
using nscreg.Utilities.Enums;

namespace nscreg.Data.Migrations
{
    [DbContext(typeof(NSCRegDbContext))]
    partial class NSCRegDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("nscreg.Data.Entities.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<int>("ActivityRevx")
                        .HasColumnName("Activity_Revx");

                    b.Property<int>("ActivityRevy")
                        .HasColumnName("Activity_Revy");

                    b.Property<int>("ActivityType")
                        .HasColumnName("Activity_Type");

                    b.Property<int>("ActivityYear")
                        .HasColumnName("Activity_Year");

                    b.Property<int>("Employees")
                        .HasColumnName("Employees");

                    b.Property<DateTime>("IdDate")
                        .HasColumnName("Id_Date");

                    b.Property<decimal>("Turnover")
                        .HasColumnName("Turnover");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnName("Updated_By");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnName("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("ActivityRevx");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("nscreg.Data.Entities.ActivityCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Section")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ActivityCategories");
                });

            modelBuilder.Entity("nscreg.Data.Entities.ActivityCategoryRole", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnName("Role_Id");

                    b.Property<int>("ActivityCategoryId")
                        .HasColumnName("Activity_Category_Id");

                    b.HasKey("RoleId", "ActivityCategoryId");

                    b.HasIndex("ActivityCategoryId");

                    b.ToTable("ActivityCategoryRoles");
                });

            modelBuilder.Entity("nscreg.Data.Entities.ActivityStatisticalUnit", b =>
                {
                    b.Property<int>("UnitId")
                        .HasColumnName("Unit_Id");

                    b.Property<int>("ActivityId")
                        .HasColumnName("Activity_Id");

                    b.HasKey("UnitId", "ActivityId");

                    b.HasIndex("ActivityId");

                    b.ToTable("ActivityStatisticalUnits");
                });

            modelBuilder.Entity("nscreg.Data.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Address_id");

                    b.Property<string>("AddressPart1")
                        .HasColumnName("Address_part1");

                    b.Property<string>("AddressPart2")
                        .HasColumnName("Address_part2");

                    b.Property<string>("AddressPart3")
                        .HasColumnName("Address_part3");

                    b.Property<string>("GpsCoordinates")
                        .HasColumnName("GPS_coordinates");

                    b.Property<int>("RegionId")
                        .HasColumnName("Region_id");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.HasIndex("AddressPart1", "AddressPart2", "AddressPart3", "RegionId", "GpsCoordinates")
                        .IsUnique();

                    b.ToTable("Address");
                });

            modelBuilder.Entity("nscreg.Data.Entities.AnalysisError", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnalysisLogId");

                    b.Property<string>("ErrorKey");

                    b.Property<string>("ErrorValue");

                    b.Property<int>("RegId");

                    b.HasKey("Id");

                    b.HasIndex("AnalysisLogId");

                    b.HasIndex("RegId");

                    b.ToTable("AnalysisErrors");
                });

            modelBuilder.Entity("nscreg.Data.Entities.AnalysisLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int?>("LastAnalyzedUnitId");

                    b.Property<DateTime?>("ServerEndPeriod");

                    b.Property<DateTime?>("ServerStartPeriod");

                    b.Property<string>("SummaryMessages");

                    b.Property<DateTime>("UserEndPeriod");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<DateTime>("UserStartPeriod");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AnalysisLogs");
                });

            modelBuilder.Entity("nscreg.Data.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("nscreg.Data.Entities.DataSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AllowedOperations");

                    b.Property<string>("AttributesToCheck");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Priority");

                    b.Property<string>("Restrictions");

                    b.Property<int>("StatUnitType");

                    b.Property<string>("UserId");

                    b.Property<string>("VariablesMapping");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("DataSources");
                });

            modelBuilder.Entity("nscreg.Data.Entities.DataSourceQueue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataSourceFileName")
                        .IsRequired();

                    b.Property<int>("DataSourceId");

                    b.Property<string>("DataSourcePath")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndImportDate");

                    b.Property<DateTime?>("StartImportDate");

                    b.Property<int>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceId");

                    b.HasIndex("UserId");

                    b.ToTable("DataSourceQueues");
                });

            modelBuilder.Entity("nscreg.Data.Entities.DataUploadingLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DataSourceQueueId");

                    b.Property<DateTime?>("EndImportDate");

                    b.Property<string>("Note");

                    b.Property<string>("SerializedUnit");

                    b.Property<DateTime?>("StartImportDate");

                    b.Property<string>("StatUnitName");

                    b.Property<int>("Status");

                    b.Property<string>("TargetStatId");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceQueueId");

                    b.ToTable("DataUploadingLogs");
                });

            modelBuilder.Entity("nscreg.Data.Entities.EnterpriseGroup", b =>
                {
                    b.Property<int>("RegId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ActualAddressId");

                    b.Property<int?>("AddressId");

                    b.Property<int>("ChangeReason")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("ContactPerson");

                    b.Property<string>("DataSource");

                    b.Property<string>("EditComment");

                    b.Property<string>("EmailAddress");

                    b.Property<int?>("Employees");

                    b.Property<DateTime?>("EmployeesDate");

                    b.Property<int?>("EmployeesYear");

                    b.Property<DateTime>("EndPeriod");

                    b.Property<string>("EntGroupType");

                    b.Property<string>("ExternalId");

                    b.Property<DateTime?>("ExternalIdDate");

                    b.Property<int?>("ExternalIdType");

                    b.Property<int?>("InstSectorCodeId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("LegalFormId");

                    b.Property<DateTime?>("LiqDateEnd");

                    b.Property<DateTime?>("LiqDateStart");

                    b.Property<string>("LiqReason");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<int?>("NumOfPeopleEmp");

                    b.Property<int?>("ParentId");

                    b.Property<int?>("ParrentRegId");

                    b.Property<int?>("PostalAddressId");

                    b.Property<DateTime>("RegIdDate");

                    b.Property<int?>("RegMainActivityId");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("RegistrationReason");

                    b.Property<DateTime?>("ReorgDate");

                    b.Property<string>("ReorgReferences");

                    b.Property<string>("ReorgTypeCode");

                    b.Property<string>("ShortName");

                    b.Property<DateTime>("StartPeriod");

                    b.Property<string>("StatId");

                    b.Property<DateTime?>("StatIdDate");

                    b.Property<string>("Status");

                    b.Property<DateTime>("StatusDate");

                    b.Property<string>("SuspensionEnd");

                    b.Property<string>("SuspensionStart");

                    b.Property<DateTime?>("TaxRegDate");

                    b.Property<string>("TaxRegId");

                    b.Property<string>("TelephoneNo");

                    b.Property<decimal?>("Turnover");

                    b.Property<DateTime?>("TurnoverDate");

                    b.Property<int?>("TurnoverYear");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<string>("WebAddress");

                    b.HasKey("RegId");

                    b.HasIndex("ActualAddressId");

                    b.HasIndex("AddressId");

                    b.HasIndex("ParrentRegId");

                    b.ToTable("EnterpriseGroups");
                });

            modelBuilder.Entity("nscreg.Data.Entities.LegalForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("LegalForms");
                });

            modelBuilder.Entity("nscreg.Data.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<int>("CountryId");

                    b.Property<string>("GivenName");

                    b.Property<DateTime>("IdDate");

                    b.Property<string>("PersonalId");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PhoneNumber1");

                    b.Property<int>("Role");

                    b.Property<byte>("Sex");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("nscreg.Data.Entities.PersonStatisticalUnit", b =>
                {
                    b.Property<int>("UnitId")
                        .HasColumnName("Unit_Id");

                    b.Property<int>("PersonId")
                        .HasColumnName("Person_Id");

                    b.Property<int>("PersonType");

                    b.HasKey("UnitId", "PersonId", "PersonType");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonStatisticalUnits");
                });

            modelBuilder.Entity("nscreg.Data.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdminstrativeCenter");

                    b.Property<string>("Code");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("nscreg.Data.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToSystemFunctions");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<string>("StandardDataAccess");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("nscreg.Data.Entities.SectorCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("SectorCodes");
                });

            modelBuilder.Entity("nscreg.Data.Entities.StatisticalUnit", b =>
                {
                    b.Property<int>("RegId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ActualAddressId");

                    b.Property<int?>("AddressId");

                    b.Property<int>("ChangeReason")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Classified");

                    b.Property<string>("ContactPerson");

                    b.Property<string>("DataSource");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("EditComment");

                    b.Property<string>("EmailAddress");

                    b.Property<int?>("Employees");

                    b.Property<DateTime?>("EmployeesDate");

                    b.Property<int?>("EmployeesYear");

                    b.Property<DateTime>("EndPeriod");

                    b.Property<string>("ExternalId");

                    b.Property<DateTime?>("ExternalIdDate");

                    b.Property<int?>("ExternalIdType");

                    b.Property<string>("ForeignParticipation");

                    b.Property<int?>("ForeignParticipationCountryId");

                    b.Property<bool>("FreeEconZone");

                    b.Property<int?>("InstSectorCodeId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastAnalysisDate");

                    b.Property<int?>("LegalFormId");

                    b.Property<string>("LiqDate");

                    b.Property<string>("LiqReason");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<int?>("NumOfPeopleEmp");

                    b.Property<int?>("ParentId");

                    b.Property<int?>("ParentOrgLink");

                    b.Property<int>("PostalAddressId");

                    b.Property<int?>("RefNo");

                    b.Property<DateTime>("RegIdDate");

                    b.Property<int?>("RegMainActivityId");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("RegistrationReason");

                    b.Property<DateTime?>("ReorgDate");

                    b.Property<string>("ReorgReferences");

                    b.Property<string>("ReorgTypeCode");

                    b.Property<string>("ShortName");

                    b.Property<DateTime>("StartPeriod");

                    b.Property<string>("StatId")
                        .HasMaxLength(15);

                    b.Property<DateTime?>("StatIdDate");

                    b.Property<int>("Status");

                    b.Property<DateTime?>("StatusDate");

                    b.Property<string>("SuspensionEnd");

                    b.Property<string>("SuspensionStart");

                    b.Property<DateTime?>("TaxRegDate");

                    b.Property<string>("TaxRegId");

                    b.Property<string>("TelephoneNo");

                    b.Property<decimal?>("Turnover");

                    b.Property<DateTime?>("TurnoverDate");

                    b.Property<int?>("TurnoverYear");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<string>("WebAddress");

                    b.HasKey("RegId");

                    b.HasIndex("ActualAddressId");

                    b.HasIndex("AddressId");

                    b.HasIndex("ForeignParticipationCountryId");

                    b.HasIndex("ParentId");

                    b.HasIndex("RegMainActivityId");

                    b.HasIndex("StatId");

                    b.ToTable("StatisticalUnits");

                    b.HasDiscriminator<string>("Discriminator").HasValue("StatisticalUnit");
                });

            modelBuilder.Entity("nscreg.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("DataAccess");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Status");

                    b.Property<DateTime?>("SuspensionDate");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("nscreg.Data.Entities.UserRegion", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("User_Id");

                    b.Property<int>("RegionId")
                        .HasColumnName("Region_Id");

                    b.HasKey("UserId", "RegionId");

                    b.HasIndex("RegionId");

                    b.ToTable("UserRegions");
                });

            modelBuilder.Entity("nscreg.Data.Entities.EnterpriseUnit", b =>
                {
                    b.HasBaseType("nscreg.Data.Entities.StatisticalUnit");

                    b.Property<bool>("Commercial");

                    b.Property<int?>("EntGroupId");

                    b.Property<DateTime>("EntGroupIdDate");

                    b.Property<string>("EntGroupRole");

                    b.Property<string>("ForeignCapitalCurrency");

                    b.Property<string>("ForeignCapitalShare");

                    b.Property<string>("MunCapitalShare");

                    b.Property<string>("PrivCapitalShare");

                    b.Property<string>("StateCapitalShare");

                    b.Property<string>("TotalCapital");

                    b.HasIndex("EntGroupId");

                    b.ToTable("EnterpriseUnits");

                    b.HasDiscriminator().HasValue("EnterpriseUnit");
                });

            modelBuilder.Entity("nscreg.Data.Entities.LegalUnit", b =>
                {
                    b.HasBaseType("nscreg.Data.Entities.StatisticalUnit");

                    b.Property<DateTime>("EntRegIdDate");

                    b.Property<int?>("EnterpriseGroupRegId");

                    b.Property<int?>("EnterpriseUnitRegId");

                    b.Property<string>("ForeignCapitalCurrency");

                    b.Property<string>("ForeignCapitalShare");

                    b.Property<string>("Founders");

                    b.Property<bool>("Market");

                    b.Property<string>("MunCapitalShare");

                    b.Property<string>("Owner");

                    b.Property<string>("PrivCapitalShare");

                    b.Property<string>("StateCapitalShare");

                    b.Property<string>("TotalCapital");

                    b.HasIndex("EnterpriseGroupRegId");

                    b.HasIndex("EnterpriseUnitRegId");

                    b.ToTable("LegalUnits");

                    b.HasDiscriminator().HasValue("LegalUnit");
                });

            modelBuilder.Entity("nscreg.Data.Entities.LocalUnit", b =>
                {
                    b.HasBaseType("nscreg.Data.Entities.StatisticalUnit");

                    b.Property<int?>("EnterpriseUnitRegId");

                    b.Property<int?>("LegalUnitId");

                    b.Property<DateTime>("LegalUnitIdDate");

                    b.HasIndex("EnterpriseUnitRegId");

                    b.HasIndex("LegalUnitId");

                    b.ToTable("LocalUnits");

                    b.HasDiscriminator().HasValue("LocalUnit");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("nscreg.Data.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("nscreg.Data.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.Activity", b =>
                {
                    b.HasOne("nscreg.Data.Entities.ActivityCategory", "ActivityRevxCategory")
                        .WithMany()
                        .HasForeignKey("ActivityRevx")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.User", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.ActivityCategoryRole", b =>
                {
                    b.HasOne("nscreg.Data.Entities.ActivityCategory", "ActivityCategory")
                        .WithMany("ActivityCategoryRoles")
                        .HasForeignKey("ActivityCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.Role", "Role")
                        .WithMany("ActivitysCategoryRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.ActivityStatisticalUnit", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Activity", "Activity")
                        .WithMany("ActivitiesUnits")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.StatisticalUnit", "Unit")
                        .WithMany("ActivitiesUnits")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.Address", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.AnalysisError", b =>
                {
                    b.HasOne("nscreg.Data.Entities.AnalysisLog", "AnalysisLog")
                        .WithMany("AnalysisErrors")
                        .HasForeignKey("AnalysisLogId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.StatisticalUnit", "StatisticalUnit")
                        .WithMany("AnalysisErrors")
                        .HasForeignKey("RegId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.AnalysisLog", b =>
                {
                    b.HasOne("nscreg.Data.Entities.User", "User")
                        .WithMany("AnalysisLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.DataSource", b =>
                {
                    b.HasOne("nscreg.Data.Entities.User", "User")
                        .WithMany("DataSources")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.DataSourceQueue", b =>
                {
                    b.HasOne("nscreg.Data.Entities.DataSource", "DataSource")
                        .WithMany("DataSourceQueuedUploads")
                        .HasForeignKey("DataSourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.User", "User")
                        .WithMany("DataSourceQueues")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.DataUploadingLog", b =>
                {
                    b.HasOne("nscreg.Data.Entities.DataSourceQueue", "DataSourceQueue")
                        .WithMany("DataUploadingLogs")
                        .HasForeignKey("DataSourceQueueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.EnterpriseGroup", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Address", "ActualAddress")
                        .WithMany()
                        .HasForeignKey("ActualAddressId");

                    b.HasOne("nscreg.Data.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("nscreg.Data.Entities.EnterpriseGroup", "Parrent")
                        .WithMany()
                        .HasForeignKey("ParrentRegId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.LegalForm", b =>
                {
                    b.HasOne("nscreg.Data.Entities.LegalForm", "Parent")
                        .WithMany("LegalForms")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.Person", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Country", "NationalityCode")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.PersonStatisticalUnit", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Person", "Person")
                        .WithMany("PersonsUnits")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.StatisticalUnit", "Unit")
                        .WithMany("PersonsUnits")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.SectorCode", b =>
                {
                    b.HasOne("nscreg.Data.Entities.SectorCode", "Parent")
                        .WithMany("SectorCodes")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.StatisticalUnit", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Address", "ActualAddress")
                        .WithMany()
                        .HasForeignKey("ActualAddressId");

                    b.HasOne("nscreg.Data.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("nscreg.Data.Entities.Country", "ForeignParticipationCountry")
                        .WithMany()
                        .HasForeignKey("ForeignParticipationCountryId");

                    b.HasOne("nscreg.Data.Entities.StatisticalUnit", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("nscreg.Data.Entities.Activity", "RegMainActivity")
                        .WithMany()
                        .HasForeignKey("RegMainActivityId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.UserRegion", b =>
                {
                    b.HasOne("nscreg.Data.Entities.Region", "Region")
                        .WithMany("UserRegions")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("nscreg.Data.Entities.User", "User")
                        .WithMany("UserRegions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("nscreg.Data.Entities.EnterpriseUnit", b =>
                {
                    b.HasOne("nscreg.Data.Entities.EnterpriseGroup", "EnterpriseGroup")
                        .WithMany("EnterpriseUnits")
                        .HasForeignKey("EntGroupId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.LegalUnit", b =>
                {
                    b.HasOne("nscreg.Data.Entities.EnterpriseGroup", "EnterpriseGroup")
                        .WithMany("LegalUnits")
                        .HasForeignKey("EnterpriseGroupRegId");

                    b.HasOne("nscreg.Data.Entities.EnterpriseUnit", "EnterpriseUnit")
                        .WithMany("LegalUnits")
                        .HasForeignKey("EnterpriseUnitRegId");
                });

            modelBuilder.Entity("nscreg.Data.Entities.LocalUnit", b =>
                {
                    b.HasOne("nscreg.Data.Entities.EnterpriseUnit", "EnterpriseUnit")
                        .WithMany("LocalUnits")
                        .HasForeignKey("EnterpriseUnitRegId");

                    b.HasOne("nscreg.Data.Entities.LegalUnit", "LegalUnit")
                        .WithMany("LocalUnits")
                        .HasForeignKey("LegalUnitId");
                });
        }
    }
}
