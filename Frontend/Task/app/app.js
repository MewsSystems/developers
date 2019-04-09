import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { PersistGate } from 'redux-persist/integration/react'
import regeneratorRuntime from 'regenerator-runtime'

import { store, persistor } from './src/store'
import ExchangeRate from './src/pages/ExchangeRate'
import { GlobalStyles } from './styles/GlobalStyles'

export const run = element => {
  console.log('App is running.')
  console.log(element)

  ReactDOM.render(
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <GlobalStyles />
        <ExchangeRate />
      </PersistGate>
    </Provider>,
    document.getElementById(element)
  )
}
