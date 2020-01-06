import { createStore, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import currencyPairsRatesReducer from "./reducers/currencyPairsRates.reducer";

const store = createStore(
    currencyPairsRatesReducer,
    applyMiddleware(thunk)
);

export default store