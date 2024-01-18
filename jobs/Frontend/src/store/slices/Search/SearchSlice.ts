import { createReducer, createSlice, PayloadAction } from '@reduxjs/toolkit';

export interface SearchSliceInterface {
  query: string | null
}

const initialState: SearchSliceInterface = {
  query: null
}

export const SearchSlice = createSlice({
  name: 'search',
  initialState,
  reducers: {
    search: (state, action: PayloadAction<string>) => {
      state.query = action.payload;
    }
  }
})

export const { search: setSearchMovieAction } = SearchSlice.actions;
