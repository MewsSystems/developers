import React from 'react';
import { Route, BrowserRouter, Routes, Navigate } from 'react-router-dom';
import MovieSearchPage from './pages/movie-search/movie-search';

const Router = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/movies" element={<MovieSearchPage />} />
        <Route path="/" element={<Navigate to="/movies" replace />} />
      </Routes>
    </BrowserRouter>
  );
};

export default Router;
