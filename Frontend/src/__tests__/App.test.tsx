import App from 'App'
import { store } from 'state/store'
import React, { Suspense } from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'

jest.unmock('react-i18next')

it('renders without crashing', () => {
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
