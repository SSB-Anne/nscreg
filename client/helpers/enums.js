export const statUnitTypes = new Map([
  [1, 'LocalUnit'],
  [2, 'LegalUnit'],
  [3, 'EnterpriseUnit'],
  [4, 'EnterpriseGroup'],
])

export const statUnitIcons = new Map([
  [1, 'suitcase'],
  [2, 'briefcase'],
  [3, 'building'],
  [4, 'sitemap'],
])

export const dataSourceOperations = new Map([[1, 'Create'], [2, 'Update'], [3, 'CreateAndUpdate']])

export const dataSourcePriorities = new Map([[1, 'Trusted'], [2, 'Ok'], [3, 'NotTrusted']])

export const dataSourceQueueStatuses = new Map([
  [1, 'InQueue'],
  [2, 'Loading'],
  [3, 'DataLoadCompleted'],
  [4, 'DataLoadCompletedPartially'],
])

export const dataSourceQueueLogStatuses = new Map([[1, 'Done'], [2, 'Warning'], [3, 'Error']])

export const personSex = new Map([[1, 'Male'], [2, 'Female']])

export const personTypes = new Map([
  [1, 'ContactPerson'],
  [2, 'Founder'],
  [3, 'Owner'],
  [4, 'Director'],
])

export const userStatuses = new Map([[0, 'Suspended'], [1, 'Active']])

export const activityTypes = new Map([
  [1, 'ActivityPrimary'],
  [2, 'ActivitySecondary'],
  [3, 'ActivityAncilliary'],
])

export const statUnitFormFieldTypes = new Map([
  [0, 'Boolean'],
  [1, 'DateTime'],
  [2, 'Float'],
  [3, 'Integer'],
  [4, 'MultiReference'],
  [5, 'Reference'],
  [6, 'String'],
  [7, 'Activities'],
  [8, 'Addresses'],
  [9, 'Persons'],
  [10, 'SearchComponent'],
])

export const statUnitSearchOptions = [
  { key: 0, text: 'None', value: undefined },
  { key: 1, text: 'StatId', value: 1 },
  { key: 2, text: 'Name', value: 2 },
  { key: 3, text: 'Turnover', value: 3 },
  { key: 4, text: 'Employees', value: 4 },
]

export const predicateFields = new Map([
  [1, 'UnitType'],
  [2, 'Region'],
  [3, 'MainActivity'],
  [4, 'Status'],
  [5, 'Turnover'],
  [6, 'TurnoverYear'],
  [7, 'Employees'],
  [8, 'EmployeesYear'],
  [9, 'FreeEconZone'],
  [10, 'ForeignParticipation'],
  [11, 'ParentId'],
  [12, 'RegId'],
  [13, 'Name'],
  [14, 'StatId'],
  [15, 'TaxRegId'],
  [16, 'ExternalId'],
  [17, 'ShortName'],
  [18, 'TelephoneNo'],
  [19, 'AddressId'],
  [20, 'EmailAddress'],
  [21, 'ContactPerson'],
])

export const predicateOperations = new Map([
  [1, 'Equal'],
  [2, 'NotEqual'],
  [3, 'GreaterThan'],
  [4, 'LessThan'],
  [5, 'GreaterThanOrEqual'],
  [6, 'LessThanOrEqual'],
])

export const predicateComparison = new Map([[1, 'And'], [2, 'Or'], [3, 'AndNot'], [4, 'OrNot']])

export const roles = {
  admin: 'Administrator',
  employee: 'Employee',
  external: 'External user',
}
