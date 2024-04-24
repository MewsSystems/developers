import { apiKey } from '../env';

export interface Movie {
  adult: boolean;
  backdrop_path: string;
  genre_ids: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export interface MovieApiResponse {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
}

interface SendRequest {
  (searchQuery: string, page?: number): Promise<MovieApiResponse>;
}

const sendRequest: SendRequest = async (searchQuery, page = 1) => {
  const url = `https://api.themoviedb.org/3/search/movie?query=${searchQuery}&page=${page}&api_key=${apiKey}`;

  const movieRequest = await fetch(url);

  if (!movieRequest.ok) {
    return Promise.reject(new Error('Something went wrong.'));
  }

  return movieRequest.json();
};

export { sendRequest };
