export type RawMovieType = {
  id: string;
  title: string;
  overview: string;
  poster_path: string;
};

export type MoviesFoundType = {
  page: number | null;
  results: RawMovieType[];
  total_pages: number | null;
  total_results: number | null;
};

export type MovieSearchStateType = {
  moviesList: RawMovieType[];
  totalPages: number | null;
  totalResults: number | null;
  currentPage: number;
  inputValue?: string;
  isLoading?: boolean;
  errorMessage?: null | string;
  visiblePages: number[];
  // cachedData: { [key: string]: RawMovieType[] };
};
