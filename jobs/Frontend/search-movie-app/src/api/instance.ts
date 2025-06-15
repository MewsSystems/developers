import axios from 'axios';
import { BASE_URL } from './constants';
import { API_KEY_MOVIE_SEARCH } from '../config';

const instance = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json',
    Authorization: `Bearer ${API_KEY_MOVIE_SEARCH} `,
  },
});

export { instance };
