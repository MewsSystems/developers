import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios from "axios";
import { BASE_API_URL } from "../../utils/api";
import { Movie } from "../../utils/types";

interface InitialBrowseMoviesState {
  pages: {
    [x: string | number]: Movie[];
  };
  totalPages: number;
  totalResults: number;
  isLoading: boolean;
  hasErrored: boolean;
  currentPage: number;
}

const initialState: InitialBrowseMoviesState = {
  pages: {},
  totalPages: 0,
  totalResults: 0,
  currentPage: 1,
  isLoading: false,
  hasErrored: false,
};

export const fetchMovies = createAsyncThunk(
  "movies/browse",
  async (pageNo: string) => {
    try {
      const response = await axios.get(`${BASE_API_URL}/discover/movie`, {
        params: {
          api_key: process.env.REACT_APP_DB_API_KEY,
          page: pageNo,
        },
      });
      console.log("totalPages = ", response.data.total_pages);

      return {
        pageData: { [pageNo]: response.data.results },
        totalPages: response.data.total_pages,
      };
    } catch (error) {
      console.log(error);
    }
  }
);

const browseMoviesState = createSlice({
  name: "movies",
  initialState,
  reducers: {
    updateCurrentPage(state, action) {
      const nextPage = action.payload;
      if (!!nextPage) {
        state.currentPage = nextPage;
      }
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchMovies.fulfilled, (state, action) => {
      if (action.payload) {
        state.totalPages =
          action.payload.totalPages > 500 ? 500 : action.payload.totalPages;
        state.pages = { ...state.pages, ...action.payload.pageData };
      }
    });
  },
});
export const { updateCurrentPage } = browseMoviesState.actions;

export default browseMoviesState.reducer;
