// eslint-disable-next-line import/named
import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { searchMoviesEndpoint } from "../services/movies-service";

export interface MovieItem {
  id: number;
  isAdultFilm: boolean;
  orginalLanguage: string;
  originalTitle: string;
  title: string;
  posterPath: string;
  releaseDate: string;
}

export interface MovieDetail {
  adult: boolean;
  budget: number;
  genres: string;
  id: number;
  originalLanguage: string;
  originalTitle: string;
  overview: string;
  popularity: number;
  posterPath: string;
  releaseDate: string;
  revenue: number;
  runtime: number;
  status: string;
  tagline: string;
  title: string;
  voteAverage: number;
  voteCount: number;
}

export interface MoviesPage {
  movies: MovieItem[];
  total: number;
  page: number;
  totalPages: number;

}

interface MoviesState {
  foundMoviesPage?: MoviesPage;
  selectedDetail?: MovieDetail;
  statusMoviesPage: "loading" | "idle"|"init";
}

const initialState: MoviesState = {
  foundMoviesPage: undefined,
  selectedDetail: undefined,
  statusMoviesPage:'init'
};

export const searchMoviesThunk = createAsyncThunk<MoviesPage,string>(
  "movies/search",
  async (query: string) => {
    const response: MoviesPage = await searchMoviesEndpoint(query);
    return response;
  }
);

export const MoviesSlice = createSlice({
  name: "movies",
  initialState,
  reducers: {
    searchMovies: (
      state: MoviesState,
      action: PayloadAction<{ results: MoviesPage }>
    ) => {
      state.foundMoviesPage = action.payload.results;
    },
    getMovieDetail: (
      state: MoviesState,
      action: PayloadAction<{ detail: MovieDetail }>
    ) => {
      state.selectedDetail = action.payload.detail;
    },
  },
  extraReducers:(builder)=>{
    builder.addCase(searchMoviesThunk.pending,(state)=>{
        state.statusMoviesPage='loading'
    });

    builder.addCase(searchMoviesThunk.fulfilled,(state,{payload})=>{
        state.statusMoviesPage='idle'
        state.foundMoviesPage=payload
    })
  }
  
});

export default MoviesSlice.reducer;
export const { searchMovies, getMovieDetail } = MoviesSlice.actions;
