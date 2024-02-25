import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Homepage from '../pages/Home';
import MovieDetail from '../pages/MovieDetail';
import { APPLICATION_URLS } from './urls';

const AppRouting = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path={APPLICATION_URLS.home} element={<Homepage />} />
        <Route path={APPLICATION_URLS.movideDetail} element={<MovieDetail />} />
      </Routes>
    </BrowserRouter>
  );
};

export default AppRouting;
