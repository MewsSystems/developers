import { createStore } from 'redux';

import currency from './reducers/currency';

export default createStore(
  currency,
);