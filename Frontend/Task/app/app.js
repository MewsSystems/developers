import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import regeneratorRuntime from 'regenerator-runtime'

import store from './src/store'
import ExchangeRate from './src/pages/ExchangeRate'
import { GlobalStyles } from './styles/GlobalStyles'

export const run = element => {
  console.log('App is running.')
  console.log(element)

  ReactDOM.render(
    <Provider store={store}>
      <GlobalStyles />
      <ExchangeRate />
    </Provider>,
    document.getElementById(element)
  )
}
