import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter, Route } from 'react-router-dom';
import { store } from './configureStore';

import Page from './pages/Page';
import RatesPage from './pages/RatesPage';

import './styles/main.scss';

const App = () => (
  <Provider store={store}>
    <BrowserRouter>
      <Page>
        <Route path="/" component={RatesPage} exact />
      </Page>
    </BrowserRouter>
  </Provider>
);

ReactDOM.render(<App />, document.getElementById('exchange-rate-client'));