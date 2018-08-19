import { combineReducers } from "redux";
import ratesReducer from "./ratesReducer";
import pairsReducer from "./currencyPairsReducer";

export default combineReducers({
  rates: ratesReducer,
  pairs: pairsReducer
});
