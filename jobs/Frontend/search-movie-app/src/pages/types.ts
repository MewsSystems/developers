import type { DetailsMovie, ListMovie } from '../types';

type CardDetailsMovieProps = {
  data: DetailsMovie;
};

type CardMovieProps = {
  data: ListMovie;
  handleOnClick: () => void;
};

export type { CardDetailsMovieProps, CardMovieProps };
