import { CssBaseline, Stack } from '@mui/material';
import { ThemeProvider } from '@mui/material/styles';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import MovieDetails from './features/movies/components/MovieDetails';
import Search from './features/search/components/Search';
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
              <Route path="/" element={<Navigate to="/search/" />} />
              <Route path="search" element={<Search />} />
              <Route path="movie/:movieId" element={<MovieDetails />} />
            </Routes>
          </BrowserRouter>
        </Stack>
      </CssBaseline>
    </ThemeProvider>
  );
}

export default App;
