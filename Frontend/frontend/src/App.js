import React from 'react';
import { ThemeProvider, } from 'styled-components';
import { Provider as ReduxProvider, } from 'react-redux';

import { theme, } from './theme';
import { configureStore, } from './store/configureStore';
import MainLayout from './modules/Main/MainLayout';


/**
 * Create Redux Store
 */
const store = configureStore();


const App = () => (
  <ReduxProvider store={store}>
    <ThemeProvider theme={theme}>

      <MainLayout />

    </ThemeProvider>
  </ReduxProvider>
);


export default App;
