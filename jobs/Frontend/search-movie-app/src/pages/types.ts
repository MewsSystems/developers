import type { DetailsMovie, ListMovie } from '../types';

type CardDetailsMovieProps = {
  data: DetailsMovie;
};

type CardMovieProps = {
  data: ListMovie;
  handleOnClick: () => void;
};

type RenderLoaderParams = { isLoading: boolean; isLoadingPopularMovies: boolean };
type RenderMovieListParams = {
  isLoading: boolean;
  isLoadingPopularMovies: boolean;
  listMovies: ListMovie[] | undefined;
  isFetchingNextPage: boolean;
  inputSearchMovie: string;
  hasNextPage: boolean;
  fetchNextPage: () => void;
};
type RenderResultNotFoundLayoutParams = {
  isLoading: boolean;
  isLoadingPopularMovies: boolean;
  listMovies: ListMovie[] | undefined;
  inputSearchDebounced: string;
};
type RenderErrorLayoutParams = { error: Error | null };

export type {
  CardDetailsMovieProps,
  CardMovieProps,
  RenderErrorLayoutParams,
  RenderLoaderParams,
  RenderMovieListParams,
  RenderResultNotFoundLayoutParams,
};
