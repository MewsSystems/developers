import React from 'react';
import {MovieSearch} from './features/movieSearch/MovieSearch';
import {Container} from '@mui/material';
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import {MovieDetail} from './features/movieDetail/MovieDetail';
import HeaderAppBar from './components/HeaderAppBar';
import NotFound from './components/NotFound';
import './i18n';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <HeaderAppBar />
        <Container maxWidth="lg">
          <Routes>
            <Route path="/" element={<MovieSearch />} />
            <Route path="movie/:movieId" element={<MovieDetail />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </Container>
      </BrowserRouter>
    </div>
  );
}

export default App;
