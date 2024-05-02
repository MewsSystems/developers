import { Box, CircularProgress, Paper, TextField, Typography } from '@mui/material';
import { DataGrid, GridColDef, GridEventListener, GridSearchIcon } from '@mui/x-data-grid';
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { ChangeEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate, useSearchParams } from 'react-router-dom';
import AppRoutes from '../../../configs/appRoutes';
import { ENDPOINT_URL_IMAGES_w500 } from '../../../configs/config';
import useDelayedRender from '../../../hooks/useDelayRender';
import { searchMovies } from '../../api/searchMovie';
import { Movie } from '../../movies/models/Movie';

export default function Search() {
  const defaultPageSize = 6;
  const { t } = useTranslation();
  const navigate = useNavigate();

  useEffect(() => {
    document.title = t('common.appTitle');
  }, [t]);

  const [searchParams, setSearchParams] = useSearchParams();
  const [query, setQuery] = useState(searchParams.get('query') || '');

  const { isLoading, data, error } = useQuery({
    queryKey: ['movies', query],
    queryFn: async () =>
      searchMovies(query)
        .then(response => response.data)
        .catch((error: AxiosError) => {
          console.error(error.toJSON());
          throw new Error(t('error.failedMoviesFetch'));
        })
  });

  const [movies, setMovies] = useState<Movie[]>([]);

  useEffect(() => {
    if (data?.results) setMovies(data.results);
  }, [data]);

  const shouldRenderError = useDelayedRender(error !== null);
  const shouldRenderNoMovies = useDelayedRender(movies.length === 0 && query !== '');
  const shouldRenderLoading = useDelayedRender(isLoading);

  const columns: GridColDef<Movie>[] = [
    {
      field: 'poster_path',
      headerName: t('movieDetails.poster'),
      sortable: false,
      width: 100,
      renderCell: params => (
        <img height={100} src={ENDPOINT_URL_IMAGES_w500 + params.value} alt={t('movieDetails.poster')} />
      )
    },
    {
      field: 'title',
      headerName: t('movieDetails.title'),
      width: 240
    },
    {
      field: 'release_date',
      headerName: t('movieDetails.releaseDate'),
      width: 155,
      valueFormatter: (value: Movie['release_date']) => value.substring(0, 4)
    },
    {
      field: 'adult',
      headerName: t('movieDetails.adult'),
      width: 100,
      valueFormatter: (value: Movie['adult']) => (value ? t('common.yes') : t('common.no'))
    },
    {
      field: 'popularity',
      headerName: t('movieDetails.popularity'),
      type: 'number',
      width: 140
    },
    {
      field: 'vote_average',
      headerName: t('movieDetails.voteAverage'),
      type: 'number',
      width: 155,
      valueFormatter: (value: Movie['vote_average']) => `${Math.round(value)}/10`
    },
    {
      field: 'vote_count',
      headerName: t('movieDetails.voteCount'),
      type: 'number',
      width: 140
    },
    {
      field: 'original_language',
      headerName: t('movieDetails.originalLanguage'),
      minWidth: 185,
      align: 'center',
      valueFormatter: (value: Movie['original_language']) => value.toUpperCase()
    }
  ];

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => setQuery(event.target.value);

  const handleSearch = () => {
    if (!query) return;
    setSearchParams({ query: query }, { replace: true });
  };

  const handleRowClick: GridEventListener<'rowClick'> = params => navigate(`${AppRoutes.Movie}/${params.row.id}`);

  return (
    <>
      <Typography sx={{ mb: 2 }} variant="h2">
        {t('common.appTitle')}
      </Typography>

      <Paper sx={{ m: 2, p: 4, borderRadius: 2, display: 'flex', alignItems: 'flex-end' }} elevation={3}>
        <GridSearchIcon sx={{ color: 'primary.main', mr: 1, my: 0.5 }} />
        <TextField
          id="movie-search-input"
          data-testid="movie-search-input"
          label="Search Title"
          sx={{ width: '40rem' }}
          variant="standard"
          value={query}
          onChange={handleChange}
          onKeyUp={handleSearch}
        />
      </Paper>

      {shouldRenderLoading && <CircularProgress sx={{ m: 4 }} />}

      <Paper sx={{ mt: 2, borderRadius: 2 }} elevation={3}>
        <Box sx={{ mt: 2 }}>
          {shouldRenderError && (
            <Typography data-testid="movie-search-error" variant="h5" color="error">
              Error: {error?.message}
            </Typography>
          )}

          {shouldRenderNoMovies && <Typography variant="h5">{t('error.noMoviesMatch')}</Typography>}
        </Box>

        {movies.length > 0 && (
          <DataGrid
            data-testid="movie-search-grid"
            sx={{
              '& .MuiDataGrid-cell:focus': {
                outline: 'none'
              },
              '& .MuiDataGrid-row:hover': { cursor: 'pointer', color: 'primary.main' }
            }}
            rows={movies}
            columns={columns}
            onRowClick={handleRowClick}
            rowHeight={100}
            pageSizeOptions={[defaultPageSize, 10, 25, 100]}
            initialState={{
              pagination: {
                paginationModel: { pageSize: defaultPageSize, page: 0 }
              }
            }}
          />
        )}
      </Paper>
    </>
  );
}
