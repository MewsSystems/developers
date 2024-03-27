import { createSlice } from "@reduxjs/toolkit";
import { MoviesState } from "./movies.slice.types";

// Define the initial state using that type
const initialState: MoviesState = {
  results: [],
  query: "",
  selectedMovie: null,
};

export const moviesSlice = createSlice({
  name: "movies",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
    setMovieResults: (state) => {
      state.results = [
        {
          id: 1,
          title: "The Shawshank Redemption",
          year: "1994",
          genre: "Drama",
          director: "Frank Darabont",
          poster:
            "https://upload.wikimedia.org/wikipedia/en/8/81/ShawshankRedemptionMoviePoster.jpg",
        },
        {
          id: 2,
          title: "The Godfather",
          year: "1972",
          genre: "Crime, Drama",
          director: "Francis Ford Coppola",
          poster:
            "https://upload.wikimedia.org/wikipedia/en/1/1c/Godfather_ver1.jpg",
        },
        {
          id: 3,
          title: "The Dark Knight",
          year: "2008",
          genre: "Action, Crime, Drama",
          director: "Christopher Nolan",
          poster:
            "https://upload.wikimedia.org/wikipedia/en/8/8a/Dark_Knight.jpg",
        },
      ];
    },
  },
});

export const { setMovieResults: increment } = moviesSlice.actions;

export default moviesSlice.reducer;
