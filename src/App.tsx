import { Routes, Route } from 'react-router-dom';
import { Home } from './pages/home/home';
import { MovieDetail } from './pages/movie-detail/movie-detail';

function App() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/movie/:id" element={<MovieDetail />} />
    </Routes>
  );
}

export default App;
