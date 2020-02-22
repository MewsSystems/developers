import App from 'App'
import { store } from 'state/store'
import React, { Suspense } from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'

jest.unmock('react-i18next')

describe('Test App', () => {
  it('Renders without crashing', () => {
    const div = document.createElement('div')

    ReactDOM.render(
      <Provider store={store}>
        <Suspense fallback="loading">
          <App />
        </Suspense>
      </Provider>,
      div
    )

    ReactDOM.unmountComponentAtNode(div)
  })
})
