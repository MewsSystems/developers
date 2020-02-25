import React from 'react'
import { History } from 'history'
import { ThemeProvider } from 'emotion-theming'
import { ConnectedRouter } from 'connected-react-router'
import { Store } from 'redux'
import { Provider } from 'react-redux'
import AppRoutes from './AppRoutes'
import lightTheme from './styles/themes/light'
import { ApplicationState } from './store'

interface AppProps {
  store: Store<ApplicationState>
  history: History
}

const App = ({ store, history }: AppProps) => {
  return (
    <Provider store={store}>
      <ConnectedRouter history={history}>
        <ThemeProvider theme={lightTheme}>
          <AppRoutes />
        </ThemeProvider>
      </ConnectedRouter>
    </Provider>
  )
}

export default App
