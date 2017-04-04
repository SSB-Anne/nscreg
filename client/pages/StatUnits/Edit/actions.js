import { createAction } from 'redux-act'
import { push } from 'react-router-redux'

import dispatchRequest from 'helpers/request'
import typeNames from 'helpers/statUnitTypes'
import { getModel as getModelFromProps, updateProperties } from 'helpers/modelProperties'
import { getSchema } from '../schema'

export const setErrors = createAction('set errors')

export const submitStatUnit = (type, data) =>
  dispatchRequest({
    url: `/api/statunits/${typeNames.get(Number(type))}`,
    method: 'put',
    body: data,
    onSuccess: (dispatch) => {
      dispatch(push('/statunits'))
    },
    onFail: (dispatch, errors) => {
      dispatch(setErrors(errors))
    },
  })

export const clear = createAction('clear')
export const fetchStatUnitSucceeded = createAction('fetch StatUnit succeeded')

export const fetchStatUnit = (type, id) =>
  dispatchRequest({
    url: `/api/StatUnits/GetUnitById/${type}/${id}`,
    onStart: (dispatch) => {
      dispatch(clear())
    },
    onSuccess: (dispatch, resp) => {
      const model = getSchema(type).cast(getModelFromProps(resp))
      const patched = {
        ...resp,
        properties: updateProperties(model, resp.properties),
      }
      dispatch(fetchStatUnitSucceeded(patched))
    },
  })

export const editForm = createAction('edit statUnit form')

export default {
  submitStatUnit,
  fetchStatUnitSucceeded,
  fetchStatUnit,
}
