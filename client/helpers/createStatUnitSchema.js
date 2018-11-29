import { number, object, string, array, bool, mixed } from 'yup'
import R from 'ramda'

import config, { getMandatoryFields } from 'helpers/config'
import { formatDate, getDate } from 'helpers/dateHelper'
import { toPascalCase } from 'helpers/string'

const { validationSettings, analysisRules } = config

const defaultDate = formatDate(new Date())
const sureString = string()
  .ensure()
  .default(undefined)
const sureDateString = string()
  .ensure()
  .default(defaultDate)
const nullableDate = mixed()
  .default(undefined)
  .test(value => value !== null)
const currentDate = mixed().default(getDate())
const positiveNum = number()
  .positive()
  .nullable(true)
  .default(undefined)
const positiveNumArray = array(positiveNum)
  .nullable(true)
  .ensure()
  .default([])
const lastYear = number()
  .positive()
  .nullable(true)
  .notRequired()
  .default(getDate()
    .subtract(1, 'years')
    .year())
const activitiesArray = array(object()).default(undefined)
const personsArray = array(object()).default(undefined)
const nullablePositiveNumArray = array(positiveNum)
  .min(0)
  .ensure()
  .default([])

const statId = name =>
  validationSettings.ValidateStatIdChecksum
    ? string().test(name, 'InvalidChecksum', (value) => {
      if (value.length === 0) {
        return false
      }
      const okpo = (Array(20).join('0') + value)
        .substr(-analysisRules.CalculationFields.StatIdMaxLength)
        .split('')
        .map(x => Number(x))
      const okpoWithoutLast = R.dropLast(1, okpo)
      const checkNumber = R.last(okpo)
      // eslint-disable-next-line no-mixed-operators
      let sum = okpoWithoutLast.map((v, i) => (i % 10 + 1) * v).reduce((a, b) => a + b, 0)
      let remainder = sum % 11

      if (remainder >= 10) {
        // eslint-disable-next-line no-mixed-operators
        sum = okpoWithoutLast.map((v, i) => (i % 10 + 3) * v).reduce((a, b) => a + b, 0)
        remainder = sum % 11
      }

      return !Number.isNaN(remainder) && (remainder === checkNumber || remainder === 10)
    })
    : sureString

const createAsyncTest = (url, { unitId, unitType }) =>
  R.memoize(value =>
    fetch(`${url}?${unitId ? `unitId=${unitId}&` : ''}unitType=${unitType}&value=${value}`).then(r => r.json()))

const base = {
  name: sureString.min(2, 'min 2 symbols').max(300, 'max 300 symbols'),
  dataSource: sureString,
  shortName: sureString,
  address: object(),
  actualAddress: object(),
  liqReason: sureString,
  liqDate: nullableDate,
  registrationReasonId: positiveNum,
  classified: bool(),
  foreignParticipationCountriesUnits: positiveNumArray,
  reorgTypeCode: sureString,
  suspensionEnd: nullableDate,
  suspensionStart: nullableDate,
  telephoneNo: sureString,
  emailAddress: sureString,
  webAddress: sureString,
  reorgReferences: positiveNum,
  reorgDate: nullableDate,
  notes: sureString,
  employeesDate: currentDate,
  employeesYear: lastYear,
  externalIdDate: nullableDate,
  statIdDate: currentDate,
  statusDate: currentDate,
  taxRegDate: nullableDate,
  turnoverDate: currentDate,
  turnoverYear: lastYear,
  statId: statId('statId'),
  taxRegId: sureString,
  regMainActivityId: positiveNum,
  externalId: sureString,
  externalIdType: positiveNum,
  numOfPeopleEmp: positiveNum,
  employees: positiveNum,
  turnover: positiveNum,
  parentOrgLink: positiveNum,
  status: sureString,
  persons: personsArray,
  size: positiveNum,
  foreignParticipationId: positiveNum,
  dataSourceClassificationId: positiveNum,
  reorgTypeId: positiveNum,
  unitStatusId: positiveNum,
}

const byType = {
  // Local Unit
  1: {
    legalUnitIdDate: nullableDate,
    legalUnitId: positiveNum,
    registrationDate: sureDateString,
    activities: activitiesArray,
    freeEconZone: bool(),
    countries: nullablePositiveNumArray,
  },

  // Legal Unit
  2: {
    entRegIdDate: currentDate,
    legalFormId: positiveNum,
    instSectorCodeId: positiveNum,
    totalCapital: sureString,
    munCapitalShare: sureString,
    stateCapitalShare: sureString,
    privCapitalShare: sureString,
    foreignCapitalShare: sureString,
    foreignCapitalCurrency: sureString,
    enterpriseUnitRegId: positiveNum,
    localUnits: nullablePositiveNumArray,
    activities: activitiesArray,
    freeEconZone: bool(),
    countries: nullablePositiveNumArray,
    market: bool(),
  },

  // Enterprise Unit
  3: {
    entGroupIdDate: nullableDate,
    instSectorCodeId: positiveNum,
    totalCapital: sureString,
    munCapitalShare: sureString,
    stateCapitalShare: sureString,
    privCapitalShare: sureString,
    foreignCapitalShare: sureString,
    foreignCapitalCurrency: sureString,
    entGroupRole: sureString,
    legalUnits: positiveNumArray,
    entGroupId: positiveNum,
    enterpriseUnitRegId: positiveNum,
    activities: activitiesArray,
    freeEconZone: bool(),
    countries: nullablePositiveNumArray,
    commercial: bool(),
  },

  // Enterprise Group
  4: {
    statId: statId('statId'),
    statIdDate: currentDate,
    taxRegId: positiveNum,
    taxRegDate: nullableDate,
    externalId: positiveNum,
    externalIdType: positiveNum,
    externalIdDate: nullableDate,
    dataSource: sureString,
    name: sureString.min(2, 'min 2 symbols').max(100, 'max 100 symbols'),
    shortName: sureString,
    telephoneNo: sureString,
    emailAddress: sureString,
    wbAddress: sureString,
    entGroupType: sureString,
    registrationDate: sureDateString,
    registrationReasonId: positiveNum,
    liqReason: sureString,
    suspensionStart: sureString,
    suspensionEnd: sureString,
    reorgTypeCode: sureString,
    reorgDate: nullableDate,
    reorgReferences: sureString,
    contactPerson: sureString,
    employees: positiveNum,
    numOfPeopleEmp: positiveNum,
    employeesYear: lastYear,
    employeesDate: currentDate,
    turnover: positiveNum,
    turnoverYear: lastYear,
    turnoverDate: currentDate,
    statusDate: currentDate,
    notes: sureString,
    enterpriseUnits: positiveNumArray,
    size: positiveNum,
    dataSourceClassificationId: positiveNum,
    reorgTypeId: positiveNum,
    unitStatusId: positiveNum,
  },
}

const configureSchema = (unitType, permissions, properties, unitId) => {
  const canRead = prop =>
    permissions.some(x => x.propertyName.split(/[.](.+)/)[1] === prop && (x.canRead || x.canWrite))
  const mandatoryFields = getMandatoryFields(unitType)
  const asyncTestFields = new Map(properties.reduce((acc, { name, validationUrl: url }) => {
    if (url) acc.push([toPascalCase(name), url])
    return acc
  }, []))

  const setRequired = ([name, rule]) => [
    name,
    mandatoryFields.includes(name) ? rule.required(`${name}IsRequired`) : rule,
  ]
  const setAsyncTest = ([name, rule]) => [
    name,
    asyncTestFields.has(name)
      ? rule.test(
        name,
        `${name}AsyncTestFailed`,
        createAsyncTest(asyncTestFields.get(name), { unitId, unitType }),
      )
      : rule,
  ]

  const updateRule = R.pipe(setRequired, setAsyncTest)

  return Object.entries({
    ...base,
    ...byType[unitType],
  }).reduce((acc, [key, rule]) => {
    const prop = toPascalCase(key)
    return canRead(prop)
      ? {
        ...acc,
        [key]: updateRule([prop, rule])[1],
      }
      : acc
  }, {})
}

export default R.pipe(configureSchema, object)
