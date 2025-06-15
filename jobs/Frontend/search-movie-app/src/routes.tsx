import { Routes, Route } from 'react-router';
import { DetailsMoviePage, ListMoviePage } from './pages';

const RoutesConfig = () => (
  <Routes>
    <Route path="/" element={<ListMoviePage />} />
    <Route path="/details/:movieId" element={<DetailsMoviePage />} />
  </Routes>
);

export { RoutesConfig };
