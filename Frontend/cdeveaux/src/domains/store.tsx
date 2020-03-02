import { createStore, applyMiddleware, compose } from 'redux';
import thunk from 'redux-thunk';

import reducers, { Dispatch } from './reducers';

const enhancers = compose(applyMiddleware<Dispatch>(
  thunk,
));

const store = createStore(
  reducers,
  {},
  enhancers,
);

export default store;
