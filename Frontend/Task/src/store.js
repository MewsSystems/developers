import appReducer from './reducers';
import { createStore, applyMiddleware, combineReducers } from "redux";
import thunk from 'redux-thunk';

function createGeneralReducer(dynamicReducers = {}){
  return combineReducers({
    app : appReducer,
    ...dynamicReducers
  });
}

function configureAppStore () {
  const appStore = createStore(createGeneralReducer(), applyMiddleware(thunk));

  appStore.dynamicReducers = {};

  appStore.injectReducer = (reducer, key) => {
    appStore.dynamicReducers[key] = reducer;
    appStore.replaceReducer(createGeneralReducer(appStore.dynamicReducers));
  }

  appStore.ejectReducer = (key) => {
    delete appStore.dynamicReducers[key];
    appStore.replaceReducer(createGeneralReducer(appStore.dynamicReducers));
  }

  return appStore;

}

export default configureAppStore();
