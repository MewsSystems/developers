import { createStore, applyMiddleware, compose } from "redux";
import thunk from "redux-thunk";
import logger from "redux-logger";
import rootReducer from "./reducers";

const initialState = {};

const middleware = [thunk, logger];

const store = createStore(
  rootReducer,
  initialState,
  compose(
    applyMiddleware(...middleware),
    window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()
  )
);

// if (module.hot) {
//   module.hot.accept("./reducers", () => {
//     const nextRootReducer = require("./reducers/index").default;
//     store.replaceReducer(nextRootReducer);
//   });
// }

export default store;
