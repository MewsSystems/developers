import './App.css';
import { Route, Routes } from 'react-router-dom';
import MovieSearch from './pages/MovieSearch/MovieSearch';
import MovieDetail from './pages/MovieDetail/MovieDetail';

function App() {
  return (
    <Routes>
      <Route path="/" element={<MovieSearch />} />
      <Route path="/:id" element={<MovieDetail />} />
    </Routes>
  );
}

export default App;
