import { api } from '../../../services/api';
import { MoviesResponse, MovieDetailResponse } from '../types';

export const fetchMovies = async (query: string, page: number = 1): Promise<MoviesResponse> => {
  const response = await api.get('/search/movie', {
    params: { query, page },
  });
  return response.data;
};

export async function fetchMovieDetail(id: number): Promise<MovieDetailResponse> {
  const response = await api.get(`/movie/${id}`);
  return response.data;
}
