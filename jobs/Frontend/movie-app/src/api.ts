import axios from "axios";
const API_KEY = import.meta.env.VITE_API_KEY;
const TMDB_BASE_URL = "https://api.themoviedb.org/3";
const TMDB_BASE_IMAGE_PATH = "https://www.themoviedb.org/t/p";
const TMDB_BACKDROP_PATH = `${TMDB_BASE_IMAGE_PATH}/original`;
const TMDB_POSTER_PATH = `${TMDB_BASE_IMAGE_PATH}/w220_and_h330_face`;
const PLACEHOLDER_POSTER_PATH = "https://placehold.co/220x330?text=";
const PLACEHOLDER_BACKDROP_PATH = "https://placehold.co/640x360?text=";

export const getPopularMovies = async (page: number) => {
  const getPopularMovies = `${TMDB_BASE_URL}/discover/movie?api_key=${API_KEY}&page=${page}`;
  const res = await axios.get(getPopularMovies);
  return res.data;
};

export const getSearchedMovies = async (query: string, page: number) => {
  const searchMoviesUrl = `${TMDB_BASE_URL}/search/movie?api_key=${API_KEY}&page=${page}&query=${query}`;
  const res = await axios.get(searchMoviesUrl);
  return res.data;
};

export const getMovieDetail = async (movieId: string | undefined) => {
  const res = await axios.get(
    `${TMDB_BASE_URL}/movie/${movieId}?api_key=${API_KEY}`
  );
  return res.data;
};

export function getPosterPath(moviePath: string) {
  return `${TMDB_POSTER_PATH}${moviePath}`;
}

export function getPlaceholderPosterPath(placeholderText: string) {
  return `${PLACEHOLDER_POSTER_PATH}${placeholderText}`;
}

export function getBackdropPath(moviePath: string) {
  return `${TMDB_BACKDROP_PATH}${moviePath}`;
}

export function getPlaceholderBackdropPath(placeholderText: string) {
  return `${PLACEHOLDER_BACKDROP_PATH}${placeholderText}`;
}
