import {CircularProgress, Grid, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useParams} from 'react-router-dom';
import {clearMovieDetail, fetchMovieDetail, getMovieDetail} from './movieDetailSlice';
import {useAppDispatch, useAppSelector} from '../../app/hooks';
import StarIcon from '@mui/icons-material/Star';
import noImageAvailable from '../../no_image_available.png';
import {useTranslation} from 'react-i18next';

import styles from './MovieDetail.module.css';
import {MovieDetailOverviewList} from './MovieDetailOverviewList';

export function MovieDetail() {
  const [loading, setLoading] = useState(false);
  const params = useParams();
  const dispatch = useAppDispatch();
  const movieDetail = useAppSelector(getMovieDetail);
  const {t} = useTranslation();

  const fetchMovie = async () => {
    if (params.movieId) {
      setLoading(true);
      await dispatch(fetchMovieDetail(parseInt(params.movieId)));
      setLoading(false);
    }
  };

  const parseReleaseDate = (releaseDate: string) => {
    return new Date(releaseDate).getFullYear();
  };

  const parseRuntime = (totalMinutes: number) => {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    let parsedTime = '';
    if (hours > 0) {
      parsedTime += `${hours}h `;
    }
    if (minutes > 0) {
      parsedTime += `${minutes}min`;
    }
    return parsedTime;
  };

  useEffect(() => {
    fetchMovie();
    return () => {
      dispatch(clearMovieDetail());
    };
  }, [params.movieId]);

  return (
    <Grid container direction="column" spacing={2} mt={1} alignItems="center">
      {loading && !movieDetail && (
        <Grid item my={2}>
          <CircularProgress />
        </Grid>
      )}
      {!loading && !movieDetail && (
        <Grid item my={2}>
          <Typography variant="h5" component="div">
            {t('movie.detail.not_found')}
          </Typography>
        </Grid>
      )}
      {movieDetail && (
        <Grid item container direction="row" spacing={2}>
          <Grid item>
            <img
              src={
                movieDetail.poster_path ? `https://image.tmdb.org/t/p/w400${movieDetail.poster_path}` : noImageAvailable
              }
              alt={movieDetail.title}
              loading="lazy"
              width="400"
            />
          </Grid>
          <Grid item xs>
            <Grid container direction="row" justifyContent="space-between" alignItems="center">
              <Grid item>
                <Grid container direction="column" spacing={1}>
                  <Grid item>
                    <Typography variant="h4" component="div">
                      {movieDetail.title}
                    </Typography>
                  </Grid>
                  <Grid item container direction="row" spacing={1} className={styles.movieTechnicalInfo}>
                    <Grid item>{parseReleaseDate(movieDetail.release_date)}</Grid>
                    {movieDetail.runtime > 0 && (
                      <>
                        <Grid item>|</Grid>
                        <Grid item>{parseRuntime(movieDetail.runtime)}</Grid>
                      </>
                    )}
                    <Grid item>|</Grid>
                    <Grid item>{movieDetail.status}</Grid>
                  </Grid>
                </Grid>
              </Grid>
              <Grid item>
                <Grid container direction="row" alignItems="center">
                  <Grid item>
                    <Typography variant="h5" component="div" className={styles.movieVoteAverage}>
                      {movieDetail.vote_average}
                    </Typography>
                  </Grid>
                  <Grid item>
                    <StarIcon color="warning" fontSize="large" />
                  </Grid>
                </Grid>
              </Grid>
            </Grid>
            <Grid container direction="column" mt={2} spacing={1}>
              <Grid item>
                <Typography variant="h6" component="div" className={styles.movieOverview}>
                  {t('movie.detail.overview')}
                </Typography>
              </Grid>
              <Grid item>
                <Typography variant="body1" component="div">
                  {movieDetail.overview}
                </Typography>
              </Grid>
            </Grid>
            <Grid container direction="column" my={2} spacing={1}>
              {movieDetail.genres.length > 0 && <MovieDetailOverviewList items={movieDetail.genres} title={t('movie.detail.genre')} />}
              {movieDetail.production_companies.length > 0 && (
                <MovieDetailOverviewList items={movieDetail.production_companies} title={t('movie.detail.companies')} />
              )}
              {movieDetail.production_countries.length > 0 && (
                <MovieDetailOverviewList items={movieDetail.production_countries} title={t('movie.detail.countries')} />
              )}
            </Grid>
          </Grid>
        </Grid>
      )}
    </Grid>
  );
}
