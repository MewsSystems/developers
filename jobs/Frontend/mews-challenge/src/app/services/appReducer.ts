import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../store';
import { Movie } from '../types';

export interface AppState {
    query: string;
    page: number;
    totalPages: number;
    movie: Movie | null;
}

const initialState: AppState = {
    query: '',
    page: 1,
    totalPages: 10,
    movie: null
};

export const appSlice = createSlice({
    name: 'app',
    initialState,
    reducers: {
        setQuery: (state, action: PayloadAction<string>) => {
            state.query = action.payload;
        },
        setPage: (state, action: PayloadAction<number>) => {
            state.page = action.payload;
        },
        setTotalPages: (state, action: PayloadAction<number>) => {
            state.totalPages = action.payload;
        },
        setMovie: (state, action: PayloadAction<Movie>) => {
            state.movie = action.payload;
        }
    },
    extraReducers: (builder) => { },
});

export const selectPage = (state: RootState) => state.app.page;
export const selectQuery = (state: RootState) => state.app.query;
export const selectTotalPages = (state: RootState) => state.app.totalPages;
export const selectMovie = (state: RootState) => state.app.movie;
export const { setPage, setQuery, setTotalPages, setMovie } = appSlice.actions
export default appSlice.reducer;