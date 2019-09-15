
import { Store, createStore, applyMiddleware } from 'redux';
import createSagaMiddleware from 'redux-saga';

import { createReducer } from './store/reducers';
import { rootSaga } from './store/sagas';
import { ApplicationState } from 'store/types';

const initialState = {} as ApplicationState;

export default function configureStore(): Store<ApplicationState> {
    const sagaMiddleware = createSagaMiddleware()

    const store = createStore(
        createReducer(),
        initialState,
        applyMiddleware(sagaMiddleware)
    )

    sagaMiddleware.run(rootSaga)
    return store
}