import { applyMiddleware, createStore } from 'redux';

import currency from './reducers/currency';
import { currencyMiddleware } from './middleware/currency';

export default createStore(
  currency,
  applyMiddleware(
    currencyMiddleware,
  ),
);