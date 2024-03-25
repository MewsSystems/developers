import { useLocation } from 'react-router';
import { Link } from 'react-router-dom';

import posterNotFound from '@/assets/posterNotFound.svg';
import { PageContainer } from '@/components/page/Page';
import { MOVIE_IMAGE_BASE_URL } from '@/const/endpoints';

import css from './movieDetail.module.css';

export const MovieDetail = () => {
  const location = useLocation();
  const movie = location.state;

  const posterSrc = movie.poster_path
    ? `${MOVIE_IMAGE_BASE_URL}${movie.poster_path}`
    : posterNotFound;

  return (
    <PageContainer>
      <div className={css.contentContainer}>
        <img src={posterSrc} />
        <div className={css.title}>TITLE: {movie.title}</div>
        <div className={css.title}>RELEASED: {movie.release_date}</div>
        <div className={css.title}>
          ORIGINAL_LANGUAGE: {movie.original_language}
        </div>
        <div className={css.description}>OVERVIEW: {movie.overview}</div>
        <div className={css.linkContainer}>
          <Link className={css.link} to="/">
            {' '}
            CLICK HERE TO GO HOME
          </Link>
        </div>
      </div>
    </PageContainer>
  );
};
