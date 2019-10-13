import React from 'react';

import CurrencyList from '../containers/currency-list';

const App = () => (
  <div className="main">
    <nav className="navbar navbar-light">
      <h5 className="navbar-brand mx-auto mb-2" href="">
        Currency Converter
      </h5>
      <div className="col-md-12 app-list p-0">
        <CurrencyList />
      </div>
    </nav>
  </div>
);

export default App;
