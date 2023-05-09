import { FETCH_MOVIES, FETCH_MOVIE_DETAIL } from "../actions/movieActions";

const initialState = {
  movies: { status: "idle", movies: [] },
  movieDetail: {},
};

export const movieReducer = (state = initialState, action: any) => {
  switch (action.type) {
    case FETCH_MOVIES:
      return { ...state, movies: action.payload };
    case FETCH_MOVIE_DETAIL:
      return { ...state, movieDetail: action.payload };
    default:
      return state;
  }
};
