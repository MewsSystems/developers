import React from 'react';
import { ThemeProvider, } from 'styled-components';
import { Provider as ReduxProvider, } from 'react-redux';

import { theme, } from './theme';
import { configureStore, } from './store/configureStore';
import RateListPage from './modules/RateList/pages/RateListPage';


/**
 * Set Redux Store
 */
const store = configureStore();


const App = () => (
  <ReduxProvider store={store}>
    <ThemeProvider theme={theme}>

      <RateListPage />

    </ThemeProvider>
  </ReduxProvider>
);


export default App;
