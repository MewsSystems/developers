import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import './App.css';
import { MovieSearcher } from './components/MovieSearcher';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { BrowserRouter, Route, Routes } from 'react-router';
import { MovieDetails } from './components/MovieDetails';

const queryClient = new QueryClient();

function App() {
  return (
    <>
      <BrowserRouter>
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <Routes>
            <Route path='/' element={<MovieSearcher />} />
            <Route path='/movies/:movieId' element={<MovieDetails />} />
          </Routes>
        </QueryClientProvider>
      </BrowserRouter>
    </>
  );
}

export default App;
