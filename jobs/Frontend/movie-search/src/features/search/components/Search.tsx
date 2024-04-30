import { Box, CircularProgress, TextField, Typography } from '@mui/material';
import { deepPurple } from '@mui/material/colors';
import { DataGrid, GridColDef, GridEventListener, GridSearchIcon } from '@mui/x-data-grid';
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { ChangeEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { ENDPOINT_URL_IMAGES_w500 } from '../../../configs/config';
import useDelayedRender from '../../../hooks/useDelayRender';
import { searchMovie } from '../../api/searchMovie';
import { Movie } from '../../movies/models/Movie';

export default function Search() {
  const defaultPageSize = 8;

  const { t } = useTranslation();

  useEffect(() => {
    document.title = t('common.appTitle');
  }, [t]);

  const [searchParams, setSearchParams] = useSearchParams();
  const [query, setQuery] = useState(searchParams.get('query') || '');
  const navigate = useNavigate();

  const { isLoading, data, error } = useQuery({
    queryKey: ['movies', query],
    queryFn: async () =>
      searchMovie(query)
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
      headerName: 'Poster',
      width: 150,
      renderCell: params => <img height={100} src={ENDPOINT_URL_IMAGES_w500 + params.value} alt="Movie poster" />
    },
    {
      field: 'title',
      headerName: 'Title',
      width: 250
    },
    {
      field: 'release_date',
      headerName: 'Release Date',
      width: 120,
      valueFormatter: (value: string) => value.substring(0, 4)
    },
    {
      field: 'adult',
      headerName: 'R-18',
      width: 60,
      valueFormatter: (value: boolean) => (value ? 'Yes' : 'No')
    },
    {
      field: 'popularity',
      headerName: 'Popularity',
      type: 'number',
      width: 120
    },
    {
      field: 'vote_average',
      headerName: 'Vote Average',
      type: 'number',
      width: 120
    },
    {
      field: 'vote_count',
      headerName: 'Vote Count',
      type: 'number',
      width: 100
    },
    {
      field: 'original_language',
      headerName: 'Original Language',
      width: 130,
      align: 'center',
      valueFormatter: (value: string) => value.toUpperCase()
    }
  ];

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => setQuery(event.target.value);

  const handleSearch = () => {
    if (!query) return;
    setSearchParams({ query: query }, { replace: true });
  };

  const handleRowClick: GridEventListener<'rowClick'> = params => navigate(`/movie/${params.row.id}`);

  return (
    <>
      <Typography sx={{ mb: 8 }} variant="h2">
        {t('common.appTitle')}
      </Typography>

      <Box sx={{ m: 2, display: 'flex', alignItems: 'flex-end' }}>
        <GridSearchIcon sx={{ color: deepPurple[500], mr: 1, my: 0.5 }} />
        <TextField
          id="movie-search-input"
          label="Search Title"
          sx={{ width: '40rem' }}
          variant="standard"
          value={query}
          onChange={handleChange}
          onKeyUp={handleSearch}
        />
      </Box>

      {shouldRenderLoading && <CircularProgress sx={{ m: 4 }} />}

      <Box sx={{ mt: 4 }}>
        <Box sx={{ mt: 2 }}>
          {shouldRenderError && (
            <Typography variant="h5" color="error">
              Error: {error?.message}
            </Typography>
          )}

          {shouldRenderNoMovies && <Typography variant="h5">{t('error.noMoviesMatch')}</Typography>}
        </Box>

        {movies.length > 0 && (
          <DataGrid
            sx={{
              '& .MuiDataGrid-cell:focus': {
                outline: 'none'
              },
              '& .MuiDataGrid-row:hover': { cursor: 'pointer' }
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
      </Box>
    </>
  );
}
