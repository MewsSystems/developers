import { applyMiddleware, createStore } from 'redux';

import { currencyMiddleware } from './middleware/currency';
import rootReducer from './reducers';

export default createStore(
  rootReducer,
  applyMiddleware(
    currencyMiddleware,
  ),
);