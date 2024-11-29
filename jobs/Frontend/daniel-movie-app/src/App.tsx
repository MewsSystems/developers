import './App.css';
import { Route, Routes } from 'react-router-dom';
import Home from './pages/Home/Home';
import MovieDetail from './pages/MovieDetail/MovieDetail';

function App() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/:id" element={<MovieDetail />} />
    </Routes>
  );
}

export default App;
