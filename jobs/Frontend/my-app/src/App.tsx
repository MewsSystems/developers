import './App.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MovieSearch } from './pages/MovieSearch';
import { MovieDetail } from './pages/MovieDetail';

const queryClient = new QueryClient();

const router = createBrowserRouter([
  {
    path: '/',
    element: <MovieSearch />,
  },
  {
    path: '/movie-detail', //add variable for movie name
    element: <MovieDetail />,
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
