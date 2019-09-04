import React from 'react';
import { BrowserRouter, Route, Switch, } from 'react-router-dom';
import CurrencyPairsSelector from '../components/CurrencyPairsSelector';
import NotFoundPage from '../components/NotFoundPage';
import Header from '../components/Header';
import AuthorPage from '../components/AuthorPage';


const AppRouter = () => (
  <BrowserRouter>
    <Header />
    <Switch>
      <Route path="/" component={CurrencyPairsSelector} exact={true} />
      <Route path="/author" component={AuthorPage} />
      <Route component={NotFoundPage} />
    </Switch>
  </BrowserRouter>
);

export default AppRouter;
