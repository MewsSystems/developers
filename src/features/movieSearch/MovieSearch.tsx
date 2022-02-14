import {CircularProgress, Grid, OutlinedInput, InputAdornment, Typography} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useAppDispatch, useAppSelector} from '../../app/hooks';
import {clearMovies, searchMovies, selectTotalPages} from '../movies/moviesSlice';
import {MoviesList} from '../movies/MoviesList';
import {MoviesListPagination} from '../movies/MoviesListPagination';
import SearchIcon from '@mui/icons-material/Search';
import {useTranslation} from 'react-i18next';

const SEARCH_DELAY = 1000;

export function MovieSearch() {
  const [keyword, setKeyword] = useState('');
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const dispatch = useAppDispatch();
  const totalPages = useAppSelector(selectTotalPages);
  const {t} = useTranslation();

  const searchMovie = async (queryPage: number) => {
    await dispatch(searchMovies({query: keyword, page: queryPage}));
    setLoading(false);
  };

  const onPageChange = (newPage: number) => {
    setLoading(true);
    setPage(newPage);
    searchMovie(newPage);
  };

  useEffect(() => {
    let timer: any = null;
    if (keyword !== '') {
      setLoading(true);
      setPage(1);
      timer = setTimeout(async () => {
        await searchMovie(1);
      }, SEARCH_DELAY);
    } else {
      setLoading(false);
      setPage(1);
      dispatch(clearMovies());
    }
    return () => {
      if (timer !== null) {
        clearTimeout(timer);
      }
    };
  }, [keyword]);

  return (
    <Grid container direction="column" spacing={2} mt={1} justifyContent="center" alignItems="center">
      <Grid item style={{textAlign: 'center'}}>
        <Typography variant="h3" component="div">
          {t('search.title')}
        </Typography>
        <Typography variant="h5" component="div">
          {t('search.subtitle')}
        </Typography>
      </Grid>
      <Grid item style={{width: '400px'}}>
        <OutlinedInput
          id="search-input"
          fullWidth
          placeholder={t('search.input.placeholder')}
          value={keyword}
          onChange={(e) => setKeyword(e.target.value)}
          startAdornment={
            <InputAdornment position="start">
              <SearchIcon />
            </InputAdornment>
          }
        />
      </Grid>
      <Grid item mt={2}>
        {loading ? <CircularProgress /> : <MoviesList />}
      </Grid>
      {!loading && keyword !== '' && totalPages > 0 &&  (
        <Grid item>
          <MoviesListPagination page={page} totalPages={totalPages} setPage={onPageChange} />
        </Grid>
      )}
    </Grid>
  );
}
