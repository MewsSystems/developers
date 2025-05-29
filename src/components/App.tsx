import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import {Suspense, lazy} from 'react';
import {BrowserRouter, Route, Routes} from 'react-router-dom';

const MoviesListPage = lazy(() => import('./MoviesListPage.tsx'));
const MovieDetailsPage = lazy(() => import('./MovieDetailsPage.tsx'));

const queryClient = new QueryClient();

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <Suspense fallback={<div>Loading...</div>}>
          <Routes>
            <Route path="/" element={<MoviesListPage />} />
            <Route path="/movies/:id" element={<MovieDetailsPage />} />
          </Routes>
        </Suspense>
      </BrowserRouter>
    </QueryClientProvider>
  );
};

App.displayName = 'AppWrapper';
export default App;
