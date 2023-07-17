export type Movie = {
  id: number;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: 139;
};

export type GetMoviesResponse = {
  results: Movie[];
  page: number;
  total_pages: number;
  total_results: number;
};

export type MovieDetails = {
  backdrop_path: string;
  budget: number;
  genres: {
    name: string;
  }[];
  homepage: string;
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: 33.62;
  poster_path: string;
  production_companies: {
    logo_path: string;
    name: string;
    origin_country: string;
  }[];
  production_countries: {
    name: string;
  }[];
  release_date: string;
  revenue: number;
  runtime: number;
  tagline: string;
  title: string;
  vote_average: number;
  vote_count: number;
};
