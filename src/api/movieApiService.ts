import axios from 'axios';

export const movieApiService = axios.create({
  baseURL: 'https://api.themoviedb.org/3',
  params: {
    api_key: import.meta.env.VITE_MOVIES_DATABASE_API_KEY,
  },
});
