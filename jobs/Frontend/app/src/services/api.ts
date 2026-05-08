import axios from 'axios';

const API_KEY = '03b8572954325680265531140190fd2a';
const BASE_URL = 'https://api.themoviedb.org/3';

export const api = axios.create({
  baseURL: BASE_URL,
  params: {
    api_key: API_KEY,
  },
});
