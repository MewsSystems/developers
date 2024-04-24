import { apiKey } from '../env';

interface Movie {
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

const jackReacher = `https://api.themoviedb.org/3/search/movie?query=Jack+Reacher&api_key=${apiKey}`;

const firstFilm = `https://api.themoviedb.org/3/movie/75780?&api_key=${apiKey}`;

const sendRequest = async (): Promise<MovieApiResponse> => {
  const movieRequest = await fetch(jackReacher);

  if (!movieRequest.ok) {
    return Promise.reject(new Error('Something went wrong.'));
  }

  return movieRequest.json();
};

export { sendRequest };
