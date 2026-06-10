import posterNotFound from '@/assets/posterNotFound.svg';
import { MOVIE_IMAGE_BASE_URL } from '@/const/endpoints';
import { Movie } from '@/types';

import css from './movieCard.module.css';

interface Props {
  movie: Movie;
}
export const MovieCard = ({ movie }: Props) => {
  const posterSrc = movie.poster_path
    ? `${MOVIE_IMAGE_BASE_URL}${movie.poster_path}`
    : posterNotFound;

  return (
    <div className={css.container}>
      <img src={posterSrc} alt="movie poster" />
    </div>
  );
};
