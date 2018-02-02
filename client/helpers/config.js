import { statUnitTypes, roles, sampleFramePredicateFields as allowedPredicateFields } from './enums'

// eslint-disable-next-line no-underscore-dangle
const config = window.__initialStateFromServer

const checkDAAByType = (target, type, write = false) =>
  JSON.parse(config.dataAccessAttributes).Permissions.some(({ PropertyName, CanRead, CanWrite }) =>
    `${statUnitTypes.get(type)}.${target}`.toLowerCase() === PropertyName.toLowerCase() &&
      (write ? CanWrite : CanRead))

export const canRead = (target, type = null) =>
  type === null
    ? [...statUnitTypes.keys()].every(x => checkDAAByType(target, x))
    : checkDAAByType(target, type)

export const canWrite = (target, type = null) =>
  type === null
    ? [...statUnitTypes.keys()].every(x => checkDAAByType(target, x, true))
    : checkDAAByType(target, type, true)

export const checkSystemFunction = target => config.systemFunctions.includes(target)

export const getMandatoryFields = unitTypeId =>
  Object.entries({
    ...config.mandatoryFields.StatUnit,
    ...config.mandatoryFields[statUnitTypes.get(Number(unitTypeId))],
  }).reduce((result, [prop, isRequired]) => {
    if (isRequired) result.push(prop)
    return result
  }, [])

export const predicateFields = new Map(Object.entries(config.sampleFramePredicateFieldMeta)
  .map(([k, v]) => [Number(k), v])
  .filter(([k]) => allowedPredicateFields.has(k)))

export const isInRole = (...userRoles) => config.roles.some(r => userRoles.some(x => x === r))

export const isAdmin = () => isInRole(roles.admin)

export default config
