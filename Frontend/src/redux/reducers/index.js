import { combineReducers} from "redux";
import detailsReducer from "./detailsReducer";
import moviesReducer from "./moviesReducer";
import paginatorReducer from "./paginatorReducer";

export default combineReducers({
    moviesReducer,
    paginatorReducer,
    detailsReducer,
    }
)
