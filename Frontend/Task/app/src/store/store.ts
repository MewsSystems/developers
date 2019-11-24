import { createStore } from "redux";
import { appReducer } from "./Reducers";

export const store = createStore(appReducer);
