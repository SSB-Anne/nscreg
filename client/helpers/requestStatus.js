import { createAction, createReducer } from 'redux-act'

export const actions = {
  dismiss: createAction('REQUEST_DISMISS'),
  failed: createAction('REQUEST_FAILED'),
  started: createAction('REQUEST_STARTED'),
  succeeded: createAction('REQUEST_SUCCEEDED'),
}

const initialState = {
  messages: undefined,
  code: 0,
}

export const reducer = createReducer(
  {
    [actions.dismiss]: () => initialState,
    [actions.failed]: (state, data) => ({ messages: data, code: -1 }),
    [actions.started]: (state, data) => ({ messages: data, code: 1 }),
    [actions.succeeded]: (state, data) => ({ messages: data, code: 2 }),
  },
  initialState,
)
