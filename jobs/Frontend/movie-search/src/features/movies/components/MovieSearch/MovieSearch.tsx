import { CircularProgress, Paper, TextField, Typography } from '@mui/material';
import { DataGrid, GridColDef, GridEventListener, GridPaginationModel, GridSearchIcon } from '@mui/x-data-grid';
import { useInfiniteQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { ChangeEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate, useSearchParams } from 'react-router-dom';
import AppRoutes from '../../../../configs/appRoutes';
import { ENDPOINT_URL_IMAGES_w92 } from '../../../../configs/config';
import useDelayedRender from '../../../../hooks/useDelayRender';
import { searchMovies } from '../../../api/searchMovie';
import Footer from '../../../common/components/Footer/Footer';
import { Movie } from '../../models/Movie';

export default function MovieSearch() {
  const defaultPageSize = 20;
  const sessionPageKey = 'lastGridPage';
  const { t } = useTranslation();
  const navigate = useNavigate();

  useEffect(() => {
    document.title = t('common.appTitle');
  }, [t]);

  const [searchParams, setSearchParams] = useSearchParams();
  const [query, setQuery] = useState(searchParams.get('query') || '');
  const [debouncedQuery, setDebouncedQuery] = useState('');
  const [movies, setMovies] = useState<Movie[]>([]);
  const [paginationModel, setPaginatioNModel] = useState<GridPaginationModel | undefined>({
    pageSize: defaultPageSize,
    page: 0
  });

  useEffect(() => {
    const storedPage = sessionStorage.getItem(sessionPageKey);
    const restoredPage = parseInt(storedPage ?? '');
    if (restoredPage) setPaginatioNModel({ ...paginationModel, page: restoredPage } as GridPaginationModel);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      setDebouncedQuery(query);
    }, 500);

    return () => clearTimeout(timeoutId);
  }, [query]);

  const { data, error, isLoading, isSuccess, hasNextPage, fetchNextPage } = useInfiniteQuery({
    queryKey: ['movies', debouncedQuery],
    queryFn: async ({ pageParam }) =>
      searchMovies(debouncedQuery, pageParam)
        .then(response => response.data)
        .catch((error: AxiosError) => {
          console.error(error.toJSON());
          throw new Error(t('error.failedMoviesFetch'));
        }),
    initialPageParam: 1,
    getNextPageParam: response => (response.page < response.total_pages ? ++response.page : undefined)
  });

  useEffect(() => {
    if (data?.pages.length) {
      const movieDataPage = data.pages.flatMap(page => page.results);

      if (movieDataPage.length !== movies.length) {
        setMovies([...movieDataPage]);

        if (hasNextPage) fetchNextPage();
      }
    }
  }, [data, fetchNextPage, hasNextPage, movies]);

  const shouldRenderError = useDelayedRender(error !== null);
  const shouldRenderNoMovies = useDelayedRender(
    !isLoading && isSuccess && !error && movies.length === 0 && query !== ''
  );
  const shouldRenderLoading = useDelayedRender(isLoading);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => setQuery(event.target.value);

  const handleSearch = () => setSearchParams({ query: query }, { replace: true });

  const handleRowClick: GridEventListener<'rowClick'> = params => {
    sessionStorage.setItem(sessionPageKey, `${paginationModel?.page}`);
    navigate(`${AppRoutes.Movie}/${params.row.id}`);
  };

  const columnsDefinition: GridColDef<Movie>[] = [
    {
      field: 'poster_path',
      headerName: t('movieDetails.poster'),
      sortable: false,
      width: 100,
      renderCell: params => (
        <img height={100} src={ENDPOINT_URL_IMAGES_w92 + params.value} alt={t('movieDetails.poster')} />
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

      {shouldRenderError && (
        <>
          <CircularProgress sx={{ m: 4 }} />
          <Paper sx={{ m: 2, p: 3, borderRadius: 2 }} elevation={3}>
            <Typography data-testid="movie-search-error" variant="h6" color="error">
              Error: {error?.message}
            </Typography>
          </Paper>
        </>
      )}

      {shouldRenderNoMovies && (
        <Paper sx={{ m: 2, p: 3, borderRadius: 2 }} elevation={3}>
          <Typography data-testid="movie-search-no-movies" variant="h6" color="info.main">
            {t('error.noMoviesMatch')}
          </Typography>
        </Paper>
      )}

      {movies.length > 0 && (
        <Paper sx={{ m: 2, borderRadius: 2, height: 'calc(100svh - 25rem)' }} elevation={3}>
          <DataGrid
            data-testid="movie-search-grid"
            sx={{
              '& .MuiDataGrid-cell:focus': {
                outline: 'none'
              },
              '& .MuiDataGrid-row:hover': { cursor: 'pointer', color: 'primary.main' }
            }}
            rows={movies}
            columns={columnsDefinition}
            onRowClick={handleRowClick}
            rowHeight={100}
            pageSizeOptions={[defaultPageSize, 50, 100]}
            paginationModel={paginationModel}
            onPaginationModelChange={setPaginatioNModel}
          />
        </Paper>
      )}
      <Footer></Footer>
    </>
  );
}
