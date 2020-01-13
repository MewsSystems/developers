import React from 'react';
import { ThemeProvider, } from 'styled-components';
import { Provider as ReduxProvider, } from 'react-redux';
import { PersistGate, } from 'redux-persist/integration/react';

import { theme, } from './theme';
import { store, persistor, } from './store/configureStore';
import MainLayout from './modules/Main/MainLayout';


const App = () => (
  <ReduxProvider store={store}>
    <PersistGate loading={null} persistor={persistor}>
      <ThemeProvider theme={theme}>

        <MainLayout />

      </ThemeProvider>
    </PersistGate>
  </ReduxProvider>
);


export default App;
