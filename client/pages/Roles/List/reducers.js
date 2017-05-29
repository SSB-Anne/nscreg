import { createReducer } from 'redux-act'

import simpleName from '../simpleRegionName'
import * as actions from './actions'

const initialState = {
  roles: [],
  totalCount: 0,
  totalPages: 0,
}

const roles = createReducer(
  {
    [actions.fetchRolesSucceeded]: (state, data) => ({
      ...state,
      roles: data.result.map(x => ({ ...x, region: { ...x.region, name: simpleName(x.region) } })),
      totalCount: data.totalCount,
      totalPages: data.totalPages,
    }),
    [actions.toggleRoleSucceeded]: (state, { id, status }) => ({
      ...state,
      roles: state.roles.map(x => x.id !== id ? x : { ...x, status }),
    }),
  },
  initialState,
)


export default {
  roles,
}
