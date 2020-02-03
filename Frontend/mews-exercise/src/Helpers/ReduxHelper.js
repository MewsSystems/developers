import { combineReducers, createStore } from "redux";

// actions.js
export const storeMovieData = data => ({
  type: "STORE_MOVIE_DATA",
  data
});

export const storeSearchBoxValue = data => ({
  type: "STORE_SEARCH_VALUE",
  data
});

export const storeSelectedMovieID = data => ({
  type: "STORE_SELECTED_MOVIE_ID",
  data
});

export const movieData = (state = {}, action) => {
  switch (action.type) {
    case "STORE_MOVIE_DATA":
      return action.data;
    default:
      return state;
  }
};

export const searchBoxValue = (state = {}, action) => {
  switch (action.type) {
    case "STORE_SEARCH_VALUE":
      return action.data;
    default:
      return state;
  }
};

export const selectedMovieId = (state = 0, action) => {
  switch (action.type) {
    case "STORE_SELECTED_MOVIE_ID":
      return action.data;
    default:
      return state;
  }
};

export const reducers = combineReducers({
  movieData,
  searchBoxValue,
  selectedMovieId
});

export function configureStore(initialState = {}) {
  return createStore(reducers, initialState);
}

export const store = configureStore();
