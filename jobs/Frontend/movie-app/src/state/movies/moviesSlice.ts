import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { MovieDetail, MovieDetailType, MovieType, MoviesReponseType, MoviesResponse } from '../../types';
import { isLeft } from "fp-ts/Either";
import { createFetchUrl } from '../../helpers';

type FetchStatus = 'loading' | 'succeeded' | 'failed';

interface MoviesState {
    popularMovies: MovieType[]
    popularMoviesPage: number
    popularMoviesTotalPages: number
    popularMoviesStatus: FetchStatus | null;

    searchQuery: string;
    searchedMovies: MovieType[]
    searchedMoviesPage: number
    searchedMoviesTotalPages: number
    searchedMoviesStatus: FetchStatus | null;

    movieDetail: {
        [movieId: string]: MovieDetailType | null
    }
    movieDetailStatus: FetchStatus | null;
}

const initialState: MoviesState = {
    popularMovies: [],
    popularMoviesPage: 0,
    popularMoviesTotalPages: 0,
    popularMoviesStatus: null,

    searchQuery: '',
    searchedMovies: [],
    searchedMoviesPage: 0,
    searchedMoviesTotalPages: 0,
    searchedMoviesStatus: null,

    movieDetail: {},
    movieDetailStatus: null,
};

export const fetchPopularMovies = createAsyncThunk('movies/fetchPopularMovies', async ({ page }: { page: number }) => {
    const url = createFetchUrl('movie/popular', { page });
    const response = await fetch(url);
    const data = await response.json();

    const decoded = MoviesResponse.decode(data);
    if (isLeft(decoded)) {
        // TODO: Report to Sentry & display actionable error to user
        throw Error('Failed to validate movies data');
    }

    return decoded.right;
});


export const fetchMovieDetail = createAsyncThunk('movies/fetchMovieDetail', async ({ movieId }: { movieId: string }) => {
    const url = createFetchUrl(`movie/${movieId}`, {});
    const response = await fetch(url);
    const data = await response.json();

    const decoded = MovieDetail.decode(data);
    if (isLeft(decoded)) {
        // TODO: Report to Sentry & display actionable error to user
        throw Error('Failed to validate movie details');
    }

    return decoded.right;
});

export const searchMovies = createAsyncThunk('movies/searchMovies', async ({ searchQuery, page }: { searchQuery: string, page: number }) => {
    const url = createFetchUrl('search/movie', { query: searchQuery, page });
    const response = await fetch(url);
    const data = await response.json();

    const decoded = MoviesResponse.decode(data);
    if (isLeft(decoded)) {
        // TODO: Report to Sentry & display actionable error to user
        throw Error('Failed to validate searched movies');
    }

    return decoded.right;
});

const moviesSlice = createSlice({
    name: 'movies',
    initialState,
    reducers: {
        setSearchQuery(state, action: PayloadAction<string>) {
            state.searchQuery = action.payload;
        },

    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchPopularMovies.pending, (state) => {
                state.popularMoviesStatus = 'loading';
            })
            .addCase(fetchPopularMovies.fulfilled, (state, action: PayloadAction<MoviesReponseType>) => {
                state.popularMoviesStatus = 'succeeded';
                if (action.payload.page === 1) {
                    state.popularMovies = action.payload.results
                    state.popularMoviesPage = action.payload.page
                    state.popularMoviesTotalPages = action.payload.total_pages
                } else {
                    state.popularMoviesPage = action.payload.page
                    state.popularMovies = [
                        ...state.popularMovies,
                        ...action.payload.results
                    ]
                }
            })
            .addCase(fetchPopularMovies.rejected, (state) => {
                state.popularMoviesStatus = 'failed';
            })
            .addCase(fetchMovieDetail.pending, (state) => {
                state.movieDetailStatus = 'loading';
            })
            .addCase(fetchMovieDetail.fulfilled, (state, action: PayloadAction<MovieDetailType>) => {
                state.movieDetailStatus = 'succeeded';
                state.movieDetail[action.payload.id] = action.payload;
            })
            .addCase(fetchMovieDetail.rejected, (state) => {
                state.movieDetailStatus = 'failed';
            })
            .addCase(searchMovies.pending, (state) => {
                state.searchedMoviesStatus = 'loading';
            })
            .addCase(searchMovies.fulfilled, (state, action: PayloadAction<MoviesReponseType>) => {
                state.searchedMoviesStatus = 'succeeded';
                if (action.payload.page === 1) {
                    state.searchedMovies = action.payload.results
                    state.searchedMoviesPage = action.payload.page
                    state.searchedMoviesTotalPages = action.payload.total_pages
                } else {
                    state.searchedMoviesPage = action.payload.page
                    state.searchedMovies = [
                        ...state.searchedMovies,
                        ...action.payload.results
                    ]
                }
            })
            .addCase(searchMovies.rejected, (state) => {
                state.searchedMoviesStatus = 'failed';
            });
    },
});

export const { setSearchQuery } = moviesSlice.actions;

export default moviesSlice.reducer;


