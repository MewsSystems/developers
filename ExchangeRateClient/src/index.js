import React, {Component} from 'react'
import {render} from 'react-dom'
import {createStore} from 'redux'
import {Provider} from 'react-redux'
import {endpoint, interval} from 'config/config'
import reducers from 'client/reducers'

class App extends Component {
  render () {
    return (
      <span>Hello world!</span>
    )
  }
}

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
