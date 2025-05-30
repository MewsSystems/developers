import axios from 'axios';
import {MOVIE_API_BASE_URL} from './constants';

export const movieApiService = axios.create({
  baseURL: MOVIE_API_BASE_URL,
  params: {
    api_key: import.meta.env.VITE_MOVIES_DATABASE_API_KEY,
  },
});
