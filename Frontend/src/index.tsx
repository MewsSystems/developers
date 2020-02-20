import { store } from 'state/store'
import initializeTracking from 'tracking'
import GlobalStyle from 'GlobalStyle'
import 'i18n'
import App from 'App'
import React, { Suspense } from 'react'
import ReactDOM from 'react-dom'
import { persistStore } from 'redux-persist'
import { PersistGate } from 'redux-persist/integration/react'
import { Provider } from 'react-redux'

initializeTracking()

ReactDOM.render(
  <Provider store={store}>
    <PersistGate loading={null} persistor={persistStore(store)}>
      <Suspense fallback={<>Loading...</>}>
        <App />
        <GlobalStyle />
      </Suspense>
    </PersistGate>
  </Provider>,
  document.getElementById('root')
)
