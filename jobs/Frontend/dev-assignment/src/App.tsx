import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Search from './pages/Search';
import MovieDetail from './pages/MovieDetail';

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Search />} />
      <Route path="/movie/:id" element={<MovieDetail />} />
    </Routes>
  );
}