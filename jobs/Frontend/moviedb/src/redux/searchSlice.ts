import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface MovieListState {
  data: IFetchData | null,
  currentPage: number,
  searchQuery: string,
  totalPages: number
}

const initialState: MovieListState = {
  data: null,
  currentPage: 1,
  searchQuery: '',
  totalPages: 1,
};

export const searchSlice = createSlice({
  name: 'movieList',
  initialState,
  reducers: {
    setQuery: (state, action: PayloadAction<string>) => {
      state.searchQuery = action.payload;
    },
    setPage: (state, action: PayloadAction<number>) => {
      state.currentPage = action.payload;
    },
    setData: (state, action: PayloadAction<IFetchData>) => {
      state.data = action.payload;
      state.totalPages = action.payload.total_pages;
    },
    clearData: (state) => {
      state.data = initialState.data;
    },
  },
});

export const {
  setQuery, setPage, setData, clearData,
} = searchSlice.actions;

export default searchSlice.reducer;
