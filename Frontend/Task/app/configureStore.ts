
import { Store, createStore, applyMiddleware } from 'redux';
import createSagaMiddleware from 'redux-saga';

import { ApplicationState, ConnectedReduxProps } from './store/types';

import { createReducer } from './store/reducers';
import { rootSaga } from './store/sagas';

const initialState = {};

export default function configureStore(): Store<any> {

    // create the redux-saga middleware
    const sagaMiddleware = createSagaMiddleware()

    const store = createStore(
        createReducer(),
        initialState,
        applyMiddleware(sagaMiddleware)
    )

    sagaMiddleware.run(rootSaga)
    return store
}