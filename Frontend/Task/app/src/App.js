import React from 'react';

import CurrencyList from './components/CurrencyList';
import { AppWrapper, Header } from './assets/Styles';

const App = () => {
  return (
    <>
      <Header>Exchange rates App</Header>
      <AppWrapper>
        <CurrencyList />
      </AppWrapper>
    </>
  );
};

export default App;
