import React from 'react';
import CurrencyList from './components/CurrencyList';

const App = () => (
  <div className="container">
    <header className="text-center">
      <h1>Exchange Rate Client</h1>
    </header>
    <section>
      <CurrencyList />
    </section>
  </div>
);

export default App;
