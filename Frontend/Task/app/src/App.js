import React from 'react';
import CurrencyList from './components/CurrencyList';
import CurrencyFilter from './components/CurrencyFilter';
import LastFetched from './components/LastFetched';
import Footer from './components/Footer';
import ErrorDisplayer from './components/ErrorDisplayer';

const App = () => (
  <div className="container">
    <header className="text-center">
      <h1 className="display-1">Currencies Fetch Client</h1>
    </header>
    <section>
      <div className="d-flex p-2 bg-dark text-white justify-content-between">
        <CurrencyFilter />
        <ErrorDisplayer />
        <LastFetched />
      </div>
      <CurrencyList />
    </section>
    <Footer />
  </div>
);

export default App;
