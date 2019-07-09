import { createStore, applyMiddleware } from 'redux'
import createSagaMiddleware from 'redux-saga'

import reducer from './reducer';
import { watchGetConfiguration, watchGetRate } from './sagas';

const sagaMiddleware = createSagaMiddleware();

const store = createStore(
	reducer,
	applyMiddleware(sagaMiddleware)
);
sagaMiddleware.run(watchGetConfiguration);
sagaMiddleware.run(watchGetRate);

export default store;
