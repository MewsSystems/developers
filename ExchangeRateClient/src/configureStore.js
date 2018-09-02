// @flow
import { createStore, applyMiddleware } from 'redux';
import { createEpicMiddleware } from 'redux-observable';
import { composeWithDevTools } from 'redux-devtools-extension';
import reduxThunk from 'redux-thunk';
import rootReducer, { StateRecord, type State } from './reducer';
import rootEpic from './epics';

const initialState: State = StateRecord();
const epicMiddleware = createEpicMiddleware();

function configureStore(state: State = initialState) {
  const store = createStore(
    rootReducer,
    state,
    composeWithDevTools(
      applyMiddleware(reduxThunk, epicMiddleware)
    )
  );

  epicMiddleware.run(rootEpic);

  return store;
}

export default configureStore;
