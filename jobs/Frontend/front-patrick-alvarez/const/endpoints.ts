export const TMDB_API_KEY = process.env.NEXT_PUBLIC_TMDB_API_KEY
export const TMDB_API_BASE_URL = 'https://api.themoviedb.org/3'
export const MOVIE_SEARCH_URL = `${TMDB_API_BASE_URL}/search/movie?api_key=${TMDB_API_KEY}`
export const MOVIE_DISCOVER_URL = `${TMDB_API_BASE_URL}/discover/movie?api_key=${TMDB_API_KEY}`
export const MOVIE_DETAILS_URL = (id: string) =>
    `${TMDB_API_BASE_URL}/movie/${id}?api_key=${TMDB_API_KEY}`
export const API_IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/w500'
