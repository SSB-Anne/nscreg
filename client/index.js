import React from 'react'
import { render } from 'react-dom'
import { AppContainer } from 'react-hot-loader'

import { getLocale } from 'helpers/locale'
import App from './App'
import configureStore from './store/configureStore'

const store = configureStore({ locale: getLocale() })
const rootNode = document.getElementById('root')

render(
  // eslint-disable-next-line react/jsx-filename-extension
  <AppContainer>
    <App store={store} />
  </AppContainer>,
  rootNode,
)

if (module.hot) {
  module.hot.accept('./App', () => {
    render(
      <AppContainer>
        <App store={store} />
      </AppContainer>,
      rootNode,
    )
  })
}
