const API_KEY = import.meta.env.VITE_TMDB_API_KEY;
const BASE_URL = "https://api.themoviedb.org/3";

export interface Movie {
  adult: boolean;
  backdrop_path: string | null;
  genre_ids: number[];
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export interface MovieResponse {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
}

export interface MovieDetails {
  adult: boolean;
  backdrop_path: string | null;
  belongs_to_collection: string | null;
  budget: number;
  genres: {
    id: number;
    name: string;
  }[];
  homepage: string;
  id: number;
  imdb_id: string | null;
  origin_country: string[];
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  production_companies: {
    id: number;
    logo_path: string | null;
    name: string;
    origin_country: string;
  }[];
  production_countries: {
    name: string;
  }[];
  release_date: string;
  revenue: number;
  runtime: number;
  spoken_languages: {
    name: string;
  }[];
  status: string;
  tagline: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export const fetchMovies = async (
  query: string,
  page: number,
): Promise<Movie[]> => {
  const response = await fetch(
    `${BASE_URL}/search/movie?api_key=${API_KEY}&query=${query}&page=${page}`,
  );
  if (!response.ok) {
    console.error("Impossible to fetch the movies", {
      query,
      response,
    });
    throw new Error(
      `Impossible to fetch movies. Status code ${response.status}`,
    );
  }
  const data = (await response.json()) as MovieResponse;
  // TODO: add runtime type checking using typia (nice to have)
  return data.results;
};

export const fetchMovieDetail = async (id: string): Promise<MovieDetails> => {
  const response = await fetch(`${BASE_URL}/movie/${id}?api_key=${API_KEY}`);
  if (!response.ok) {
    console.error("Impossible to fetch the movie details", {
      id,
      response,
    });
    throw new Error(
      `Impossible to fetch movie details. Status code ${response.status}`,
    );
  }
  const data = (await response.json()) as MovieDetails;
  return data;
};
