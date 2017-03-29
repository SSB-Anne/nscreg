import { createAction } from 'redux-act'

import dispatchRequest from 'helpers/request'

export const fetchRolesSucceeded = createAction('fetch roles succeeded')

const fetchRoles = () =>
  dispatchRequest({
    queryParams: {
      onlyActive: 'false',
    },
    onSuccess: (dispatch, resp) => {
      dispatch(fetchRolesSucceeded(resp))
    },
  })

export const toggleRoleSucceeded = createAction('toggle role succeeded')

const toggleRole = (id, toggle) =>
  dispatchRequest({
    url: `/api/roles/${id}`,
    queryParams: {
      status: toggle,
    },
    method: 'delete',
    onSuccess: (dispatch) => {
      dispatch(toggleRoleSucceeded({ id, status: toggle }))
    },
  })

export default {
  fetchRoles,
  toggleRole,
}
