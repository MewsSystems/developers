import reducer from "./reducer/index.js";
import { applyMiddleware, compose, configureStore } from "@reduxjs/toolkit";
import { thunk } from "redux-thunk";
import logger from "redux-logger";

// eslint-disable-next-line no-unused-vars
import { Store } from "@reduxjs/toolkit";

/** @type {Store} */
let store;

if (process.env.NODE_ENV === "production") {
    store = configureStore(
      { reducer },
      compose(applyMiddleware(thunk))
    );
   } else {
    store = configureStore(
      { reducer },
      compose(applyMiddleware(logger, thunk))
    );
   }
   
   
   export default store;