import React from 'react';
import { Route, BrowserRouter, Routes, Navigate } from 'react-router-dom';
import MovieSearchPage from './pages/movie-search/movie-search';
import MovieDetail from './pages/movie-detail/movie-detail';
import NotFound from './pages/not-found/not-found';
import Header from './shared/components/header/header';

const Router = () => {
  return (
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/movies" element={<MovieSearchPage />} />
        <Route path="/movie/:id" element={<MovieDetail />} />
        <Route path="/" element={<Navigate to="/movies" replace />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
};

export default Router;
