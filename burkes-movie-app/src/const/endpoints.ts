const API_KEY = import.meta.env.VITE_MEWS_MOVIE_DB_API_KEY;

export const MOVIE_DB_BASE_URL = 'https://api.themoviedb.org';

export const MOVIE_DATA_URL = `${MOVIE_DB_BASE_URL}/3/search/movie?api_key=${API_KEY}`;
export const MOVIE_IMAGE_BASE_URL = `https://image.tmdb.org/t/p/w200`;
