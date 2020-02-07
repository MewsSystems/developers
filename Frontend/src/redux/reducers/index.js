import { combineReducers} from "redux";

import moviesReducer from "./moviesReducer";
import paginatorReducer from "./paginatorReducer";

export default combineReducers({
    moviesReducer,
    paginatorReducer,
    }
)
