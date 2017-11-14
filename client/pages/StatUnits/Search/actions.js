import { createAction } from 'redux-act'

import dispatchRequest from 'helpers/request'
import { updateFilter, setQuery } from '../actions'

export const fetchDataSucceeded = createAction('fetch StatUnits succeeded')

export const clear = createAction('clear formData filter')

const fetchData = ({ ...filter }) =>
  dispatchRequest({
    url: '/api/statunits',
    queryParams: { ...filter },
    onSuccess: (dispatch, resp) => {
      dispatch(fetchDataSucceeded({ ...resp, queryObj: this.queryParams }))
    },
  })

const deleteStatUnit = (type, id, queryParams) =>
  dispatchRequest({
    url: `/api/statunits/${type}/${id}`,
    method: 'delete',
    onSuccess: (dispatch) => {
      dispatch(fetchData(queryParams))
    },
  })

export default {
  updateFilter,
  setQuery,
  fetchData,
  deleteStatUnit,
  clear,
}
