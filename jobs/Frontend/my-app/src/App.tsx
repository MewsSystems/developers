import './App.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MovieSearch } from './pages/MovieSearch';
import { MovieDetailPage } from './pages/MovieDetailPage';

const router = createBrowserRouter([
  {
    path: '/',
    element: <MovieSearch />,
  },
  {
    path: '/movie-detail', //add variable for movie name
    element: <MovieDetailPage />,
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
