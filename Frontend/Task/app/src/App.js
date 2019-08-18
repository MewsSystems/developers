import React from 'react';
import { Wrapper, GlobalStyles } from './App.styles'
import CurrencyPairsRateList from './Components/CurrencyPairsRateList/CurrencyPairsRateList';
import CurrencyPairsFilter from './Components/CurrencyPairsFilter/CurrencyPairsFilter';
import Error from './Components/Error/Error';

function App() {
    return (
      <Wrapper>
        <GlobalStyles />
        <h1>Mews frontend developer task</h1>
        <div className="Container">
          <CurrencyPairsRateList />
          <CurrencyPairsFilter />
        </div>
        <Error />
      </Wrapper>
    );  
}
export default App;

