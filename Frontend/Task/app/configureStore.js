import thunkMiddleware from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension';
import { createStore, applyMiddleware, combineReducers } from 'redux';
import reducers from './redux/reducers';
import configurationThunks from './redux/thunks/configurationThunks';

const enhancers = applyMiddleware(thunkMiddleware);

const rootReducer = combineReducers({ ...reducers });
export const store = createStore(
  rootReducer,
  composeWithDevTools(enhancers)
);

// fetch configuration when app started
store.dispatch(configurationThunks.fetchConfiguration());
