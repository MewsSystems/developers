import { createStore, applyMiddleware } from "redux";
/* 
import { compose } from "redux";
import createSagaMiddleware from "redux-saga"; */
import { rootReducer } from "./reducers";
/* import rootSaga from "./sagas";

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const sagaMiddleware = createSagaMiddleware(); */

export default function configureStore() {
  const store = createStore(
    rootReducer /* ,
    composeEnhancers(applyMiddleware(sagaMiddleware)) */
  );

  /*   sagaMiddleware.run(rootSaga); */

  return store;
}
