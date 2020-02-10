import { combineReducers, Action } from "redux";
import { ThunkAction } from "redux-thunk";
import { movieSearchReducer } from "./movie-search";
import { movieDetailReducer } from "./movie-detail";

export type AppThunk<T = void> = ThunkAction<T, AppState, null, Action<string>>

export const reducers = combineReducers({
  movieSearch: movieSearchReducer,
  movieDetail: movieDetailReducer
});

export type AppState = ReturnType<typeof reducers>;
