import { CssBaseline, Stack } from '@mui/material';
import { ThemeProvider } from '@mui/material/styles';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import AppRoutes from './configs/appRoutes';
import PageNotFound from './features/error/components/PageNotFound';
import MovieDetails from './features/movies/components/MovieDetails/MovieDetails';
import MovieSearch from './features/movies/components/MovieSearch/MovieSearch';
import { theme } from './themes/theme';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline>
        <Stack
          sx={{
            marginTop: 'calc(100svh / 32)'
          }}
          direction="column"
          alignItems="center"
          justifyContent="center">
          <BrowserRouter>
            <Routes>
              <Route path="/" element={<Navigate to={AppRoutes.Search} />} />
              <Route path={AppRoutes.Search} element={<MovieSearch />} />
              <Route path={`${AppRoutes.Movie}/:movieId`} element={<MovieDetails />} />
              <Route path={AppRoutes.PageNotFound} element={<PageNotFound />} />
              <Route path="*" element={<PageNotFound />} />
            </Routes>
          </BrowserRouter>
        </Stack>
      </CssBaseline>
    </ThemeProvider>
  );
}

export default App;
