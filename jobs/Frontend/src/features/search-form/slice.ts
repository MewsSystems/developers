import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { SearchState } from "../../state/model";
import { MoviesDBService } from "services/movies-db";
import { LoggerService } from "services/logger";

const initialState: SearchState = {
    status: 'idle',
    keyword: '',
    movies: [],
    page: 1,
    totalPages: 0
};

export const searchFormSlice = createSlice({
    name: 'search',
    initialState,
    reducers: {
        setKeyword: (state, action) => {
            state.keyword = action.payload;
        },
        clearResults: (state) => {
            state.movies = [];
            state.page = 1;
            state.totalPages = 0;
        },
    },
    extraReducers: builder => {
        builder.addCase(getMoviesList.fulfilled, (state, action) => {
            state.status = 'idle';
            state.movies = action.payload.results;
            state.page = action.payload.page;
            state.totalPages = action.payload.total_pages;
        });
        builder.addCase(getMoviesList.rejected, (state, action) => {
            state.status = 'failed';
            state.totalPages = 0;
            LoggerService.logError(action.error);
        });
        builder.addCase(getMoviesList.pending, (state, action) => {
            state.status = 'loading';
        });
    }
});

export const { setKeyword, clearResults } = searchFormSlice.actions;

export default searchFormSlice.reducer;

export const selectState = (state: { search: SearchState; }) => state.search;

export const getMoviesList = createAsyncThunk(
    'search/getMoviesList',
    async (params: { query: string, page: number }) => {
        const response = await MoviesDBService.searchMovies(params.query, params.page);
        return response;
    }
);
