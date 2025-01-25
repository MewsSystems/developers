export const TMDB_API_KEY = process.env.NEXT_PUBLIC_TMDB_API_KEY
export const TMDB_API_BASE_URL = 'https://api.themoviedb.org/3'
export const MOVIE_SEARCH_URL = `${TMDB_API_BASE_URL}/search/movie?api_key=${TMDB_API_KEY}`
export const MOVIE_DISCOVER_URL = `${TMDB_API_BASE_URL}/discover/movie?api_key=${TMDB_API_KEY}`
