import React from 'react'
import {render} from 'react-dom'
import {createStore} from 'redux'
import {Provider} from 'react-redux'
import reducers from 'client/reducers'
import 'babel-polyfill'

import App from 'client/App'

export function run (elementId) {
  if (elementId == null) { // eslint-disable-line eqeqeq
    return false
  }

  const store = createStore(reducers)
  const targetElement = document.querySelector(elementId)

  render(
    <Provider store={store}>
      <App />
    </Provider>,
    targetElement,
  )
}
