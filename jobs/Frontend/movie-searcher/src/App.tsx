import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import './App.css';
import { MovieSearcher } from './components/MovieSearcher';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

const queryClient = new QueryClient();

function App() {
  return (
    <>
      <QueryClientProvider client={queryClient}>
        <ReactQueryDevtools initialIsOpen={false} />
        <MovieSearcher />
      </QueryClientProvider>
    </>
  );
}

export default App;
