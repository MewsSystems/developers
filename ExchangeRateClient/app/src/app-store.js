import {compose, applyMiddleware, createStore} from 'redux';
import {persistStore, autoRehydrate} from 'redux-persist';
import rootReducer from './reducers';
import {rateWatcher} from './middleware';
import config from './config';


const store = createStore(
  rootReducer,
  undefined,
  compose(
    applyMiddleware(rateWatcher({interval: config.interval})),
    autoRehydrate(),
    window.devToolsExtension ? window.devToolsExtension() : f => f
  )
);

persistStore(store, {
  whitelist: ['config', 'filter'],
  debounce: config.syncInterval,
});

export default store;
