import KeyboardArrowLeftIcon from '@mui/icons-material/KeyboardArrowLeft';
import KeyboardArrowRightIcon from '@mui/icons-material/KeyboardArrowRight';
import SearchIcon from '@mui/icons-material/Search';
import {
  Button,
  CircularProgress,
  Divider,
  FormControl,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  SelectChangeEvent,
  Stack,
  TextField,
  Typography
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { ChangeEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSearchParams } from 'react-router-dom';
import useDelayedRender from '../../../../hooks/useDelayRender';
import { searchMovies } from '../../../api/searchMovie';
import Footer from '../../../common/components/Footer/Footer';
import { Movie } from '../../models/Movie';
import EnhancedTable from './EnhancedTable/EnhancedTable';

export default function MovieSearch() {
  const { t } = useTranslation();

  useEffect(() => {
    document.title = t('common.appTitle');
  }, [t]);

  const [searchParams, setSearchParams] = useSearchParams();
  const [query, setQuery] = useState(searchParams.get('query') || '');
  const [debouncedQuery, setDebouncedQuery] = useState('');
  const [page, setPage] = useState(searchParams.get('page') || '1');
  const [pageRange, setPageRange] = useState<number[]>([]);
  const [movies, setMovies] = useState<Movie[]>([]);

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      setDebouncedQuery(query);
    }, 500);

    return () => clearTimeout(timeoutId);
  }, [query]);

  const { data, error, isLoading, isFetching } = useQuery({
    queryKey: ['movies', debouncedQuery, page],
    queryFn: async () =>
      searchMovies(debouncedQuery, page)
        .then(response => response.data)
        .catch((error: AxiosError) => {
          console.error(error.toJSON());
          throw new Error(t('error.failedMoviesFetch'));
        })
  });

  useEffect(() => {
    setMovies(data?.results ?? []);
    setPageRange(Array.from({ length: data?.total_pages ?? 0 }, (_, i) => i + 1));
  }, [data]);

  const shouldRenderError = useDelayedRender(error !== null);
  const shouldRenderLoading = useDelayedRender(isLoading);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => setQuery(event.target.value);
  const handleSearch = () => setSearchParams({ query: query, page: page }, { replace: true });

  const setNewPage = (page: string) => {
    setPage(page);
    setSearchParams({ query: query, page: page });
  };

  const handleNextPage = () => {
    if (data && data?.page < data?.total_pages) setNewPage(`${++data.page}`);
  };

  const handlePreviousPage = () => {
    if (data && data?.page > 1) setNewPage(`${--data.page}`);
  };

  const handleSelectChange = (event: SelectChangeEvent) => setNewPage(event.target.value);

  return (
    <>
      <Typography sx={{ mb: 2 }} variant="h2">
        {t('common.appTitle')}
      </Typography>

      <Paper sx={{ m: 3, p: 4, borderRadius: 2, display: 'flex', alignItems: 'flex-end' }} elevation={3}>
        <SearchIcon sx={{ color: 'primary.main', mr: 1, my: 0.5 }} />
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

      {movies.length > 0 && (
        <Stack direction="column" spacing={2} alignItems="center" divider={<Divider orientation="vertical" />}>
          <EnhancedTable rows={movies} />

          {isFetching && <CircularProgress sx={{ m: 4 }} />}

          {!isFetching && (
            <Stack direction="row" spacing={6} alignItems="center">
              <Button
                sx={{ width: 175, height: 45 }}
                size="large"
                variant="outlined"
                startIcon={<KeyboardArrowLeftIcon />}
                onClick={handlePreviousPage}
                disabled={data?.page === 1}>
                {t('common.previousPage')}
              </Button>

              <FormControl variant="filled" sx={{ m: 1, minWidth: 80 }} size="small">
                <InputLabel id="movie-search-page-select-label">{`/ ${data?.total_pages}`}</InputLabel>
                <Select
                  id="movie-search-page-select"
                  labelId="movie-search-page-select-label"
                  value={page}
                  label="Page"
                  onChange={handleSelectChange}>
                  {pageRange.map(page => (
                    <MenuItem value={page}>{page}</MenuItem>
                  ))}
                </Select>
              </FormControl>

              <Button
                sx={{ width: 175, height: 45 }}
                size="large"
                variant="outlined"
                endIcon={<KeyboardArrowRightIcon />}
                onClick={handleNextPage}
                disabled={data?.page === data?.total_pages}>
                {t('common.nextPage')}
              </Button>
            </Stack>
          )}
        </Stack>
      )}

      <Footer />
    </>
  );
}
