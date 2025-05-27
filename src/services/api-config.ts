const API_URL = import.meta.env.VITE_TMDB_API_URL;
const API_KEY = import.meta.env.VITE_TMDB_API_KEY;

if (!API_URL || !API_KEY) {
  throw new Error('Missing required environment variables for TMDB API configuration');
}

export const API_CONFIG = {
  baseUrl: API_URL,
  headers: {
    accept: 'application/json',
    Authorization: `Bearer ${API_KEY}`,
  },
} as const;
