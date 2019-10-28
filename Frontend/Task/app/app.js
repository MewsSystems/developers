import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { PersistGate } from 'redux-persist/integration/react'

import { persistor, store } from './store'

import HomePage from './pages'
import Loader from './components/shared/loader'

const App = () => (
  <Provider store={store}>
    <PersistGate loading={<Loader />} persistor={persistor}>
      <HomePage />
    </PersistGate>
  </Provider>
)

ReactDOM.render(<App />, document.getElementById('exchange-rate-client'))
