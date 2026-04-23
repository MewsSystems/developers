import axios from 'axios';

const API_KEY = '03b8572954325680265531140190fd2a';
const BASE_URL = 'https://api.themoviedb.org/3';

export const searchMovies = async (query: string, page: number = 1) => {
  const response = await axios.get(`${BASE_URL}/search/movie`, {
    params: {
      api_key: API_KEY,
      query,
      page,
    },
  });
  return response.data;
};

export const getMovieDetails = async (id: number) => {
  const response = await axios.get(`${BASE_URL}/movie/${id}`, {
    params: {
      api_key: API_KEY,
    },
  });
  return response.data;
};