export const BASE_API_URL = "https://api.themoviedb.org/3";
export const BASE_IMG_URL = "http://image.tmdb.org/t/p/w1280";
export const API_KEY = process.env.REACT_APP_DB_API_KEY;

export const endpoints = {
  searchMovie: `/search/movie`,
  discoverMovies: `/discover/movie`,
};
