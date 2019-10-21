import React from 'react';

import CurrencyList from './currency-list';

const App = () => (
  <div className="main">
    <nav className="navbar navbar-light">
      <h5 className="navbar-brand mx-auto mb-2" href="">
        Currency Converter
      </h5>
      <form className="form-inline my-2 my-lg-0">
        <div className="form-control mr-sm-2">
          <i className="fas fa-search" />
          <input
            type="text"
            className="ml-2 border-0"
            placeholder="Currency Pair Filter"
            onChange={event => this.dispatch.onTextChange(event.target.value)}
          />
        </div>
      </form>
      <a href="xxx" target="_blank">
        <i className="fab fa-github fa-2x" />
      </a>
      <div className="col-md-12 app-list p-0">
        <CurrencyList />
      </div>
    </nav>
  </div>
);

export default App;
