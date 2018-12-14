import { createStore as _createStore, applyMiddleware, compose } from "redux";
import { persistStore } from "redux-persist";
import ReduxThunk from "redux-thunk";
import rootReducer from "./modules";

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

export const store = _createStore(
  rootReducer,
  {},
  composeEnhancers(applyMiddleware(ReduxThunk))
);
export const persistor = persistStore(store);
