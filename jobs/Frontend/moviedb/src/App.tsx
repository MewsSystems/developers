import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import Detail from './pages/Detail';
import Error404 from './pages/Error404';

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/movie/:movieId" element={<Detail />} />
      <Route path="*" element={<Error404 />} />
    </Routes>
  );
}
