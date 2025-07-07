// Base Movie type (used in both search and popular)
export interface Movie {
  id: number;
  title: string;
  overview: string;
  release_date: string;
  poster_path: string | null;
  vote_average: number;
  popularity: number;
  runtime: number;
  tagline: string;
}

// Used in getPopularMovies and searchMovieList
export interface PaginatedMovieApiResponse {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
}

export type MovieContextType = {
  movies: Movie[]; // Movie list
  movieDetails: Movie; // Details of a single movie
  loading: boolean;
  error: string | null;
  searchQuery: string;
  setSearchQuery: (query: string) => void;
  setCurrentPage: (page: number) => void; // Function to set the current page
  searchMovies: () => Promise<void>; // Function to search movies
  searchMoviesByQuery: (query: string, current_page: number) => Promise<void>; // Function to search movies by query
  getMovieInfo: (movieId: number) => Promise<void>; // Function to get movie details
  itemsPerPage: number;
  currentPage: number;
  totalPages: number;
}