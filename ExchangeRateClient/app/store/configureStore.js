import { createStore, applyMiddleware, compose } from 'redux';
import logger from 'redux-logger';
import thunk from 'redux-thunk';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import rootReducer from './rootReducer';
import initialState from './initialState';

const middleware = [thunk, logger];

const persistConfig = {
    key: 'root',
    storage,
    blacklist: ['errorMessages'],
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const store = createStore(persistedReducer, initialState, composeEnhancers(applyMiddleware(...middleware)));
const persistor = persistStore(store);

export { store, persistor };
