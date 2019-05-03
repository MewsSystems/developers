import React from 'react';
import CurrencyList from './components/CurrencyList';
import CurrencyFilter from './components/CurrencyFilter';

// TODO: lastUpdatedTime and error message show

const App = () => (
  <div className="container">
    <header className="text-center">
      <h1>Exchange Rate Client</h1>
    </header>
    <section>
      <CurrencyFilter />
      <CurrencyList />
    </section>
  </div>
);

export default App;
