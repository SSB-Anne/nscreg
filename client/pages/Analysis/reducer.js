import { createReducer } from 'redux-act'

import actions from './actions'

const defaultState = {
  queue: {
    formData: {},
    items: [],
    totalCount: 0,
    fetching: false,
    error: undefined,
  },
}

const queueHandlers = {
  [actions.fetchQueueSucceeded]: (state, data) => ({
    ...state,
    queue: {
      ...state.queue,
      items: data.items,
      totalCount: data.totalCount,
      fetching: false,
      error: undefined,
    },
  }),

  [actions.fetchQueueFailed]: (state, data) => ({
    ...state,
    list: {
      ...state.list,
      data: undefined,
      fetching: false,
      error: data,
    },
  }),

  [actions.fetchQueueStarted]: state => ({
    ...state,
    list: {
      ...state.list,
      fetching: true,
      error: undefined,
    },
  }),

  [actions.updateQueueFilter]: (state, data) => ({
    ...state,
    list: {
      ...state.list,
      formData: { ...state.formData, ...data },
    },
  }),
}

export default createReducer(
  {
    ...queueHandlers,
    [actions.clear]: () => defaultState,
  },
  defaultState,
)
