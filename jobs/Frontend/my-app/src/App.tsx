import './App.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MovieSearch } from './pages/MovieSearch';
import { MovieDetailPage } from './pages/MovieDetailPage';

const queryClient = new QueryClient();

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
  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  );
}

export default App;
