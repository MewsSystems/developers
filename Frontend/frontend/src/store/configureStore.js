import { createStore, applyMiddleware, compose, } from 'redux';
import thunkMiddleware from 'redux-thunk';
import { persistStore, } from 'redux-persist';

import rootReducer from './configureReducers';


// Redux Dev tools
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;


// Redux: Store
const store = createStore(
  rootReducer,
  composeEnhancers(
    applyMiddleware(
      thunkMiddleware,
    ),
  )
);


// Middleware: Redux Persist Persister
const persistor = persistStore(store);


export {
  store,
  persistor,
};
