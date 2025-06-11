import React, { useRef } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Search from './components/views/Search';
import MovieDetail from './components/views/MovieDetail';
import Header from './components/Header';

const App = () => {
  return (
    <Router>
      <div>
        <Header></Header>
        <Routes>
          <Route path="/" element={<Search/>} />
          <Route path="/movie/:id" element={<MovieDetail/>} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
