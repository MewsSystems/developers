import { Box, CircularProgress, TextField, Typography } from '@mui/material';
import { deepPurple } from '@mui/material/colors';
import { DataGrid, GridColDef, GridEventListener, GridSearchIcon } from '@mui/x-data-grid';
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { ChangeEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { ENDPOINT_URL_IMAGES_w500 } from '../../../configs/config';
import { searchMovie } from '../../api/searchMovie';
import { Movie } from '../../movies/models/Movie';

export default function Search() {
  const { t } = useTranslation();

  useEffect(() => {
    document.title = t('common.appTitle');
  }, [t]);

  const navigate = useNavigate();
  const [query, setQuery] = useState<string>('');
  const [movies, setMovies] = useState<Movie[]>([]);

  const { isLoading, data, error } = useQuery({
    queryKey: ['movies', query],
    queryFn: async () =>
      searchMovie(query)
        .then(response => response.data)
        .catch((error: AxiosError) => {
          console.error(error.toJSON());
          throw new Error(`Failed to fetch movies`);
        })
  });

  useEffect(() => {
    if (data?.results) setMovies(data.results);
  }, [data]);

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

  const handleRowClick: GridEventListener<'rowClick'> = params => navigate(`/movie/${params.row.id}`);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => setQuery(event.target.value);

  const handleSearch = () => navigate(`/search?query=${query}`);

  const defaultPageSize = 8;

  return (
    <>
      <Box sx={{ m: 2, display: 'flex', alignItems: 'flex-end' }}>
        <GridSearchIcon sx={{ color: deepPurple[500], mr: 1, my: 0.5 }} />
        <TextField
          label="Search Title"
          sx={{ width: '40rem' }}
          variant="standard"
          value={query}
          onChange={handleChange}
          onKeyUp={handleSearch}
        />
      </Box>
      {isLoading && <CircularProgress />}
      {error && (
        <Typography variant="body2" color="error">
          Error: {error.message}
        </Typography>
      )}
      {movies.length > 0 && (
        <DataGrid
          sx={{
            '& .MuiDataGrid-cell:focus': {
              outline: 'none'
            }
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
    </>
  );
}
