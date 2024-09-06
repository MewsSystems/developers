import { MovieDetails, MovieResponse } from '@/models/movie';
import axios from 'axios';

export const fetchMovieDetails = async (id: string): Promise<MovieDetails> => {
  const { data } = await axios.get(`https://api.themoviedb.org/3/movie/${id}`, {
    params: {
      api_key: process.env.NEXT_PUBLIC_TMDB_API_KEY,
    },
  });
  return data;
};

export const fetchMovies = async (query: string, page: number = 1): Promise<MovieResponse> => {
  const { data } = await axios.get(`https://api.themoviedb.org/3/search/movie`, {
    params: {
      api_key: process.env.NEXT_PUBLIC_TMDB_API_KEY,
      query: query,
      page: page,
    },
  });

  return data;
};

