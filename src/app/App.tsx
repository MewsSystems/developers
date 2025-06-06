import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import {lazy, Suspense} from 'react';
import {ErrorBoundary} from 'react-error-boundary';
import {BrowserRouter} from 'react-router-dom';
import ErrorBoundaryFallback from './components/ErrorBoundaryFallback/ErrorBoundaryFallback';
import Loader from './components/Loader/Loader';

const AppRoutes = lazy(() => import('../routes/AppRoutes'));

export default function App() {
  const queryClient = new QueryClient();

  const clearRelevantQueries = (queryClient: QueryClient) => {
    queryClient.removeQueries({
      predicate: (query) => {
        if (!query.isActive()) return false;

        const queryKey = query.queryKey[0];
        return ['movie', 'movies'].includes(queryKey as string);
      },
    });
  };

  return (
    // pass the "future" prop to suppress warnings in the browser console from React Router about upcoming changes in v7
    <BrowserRouter future={{v7_startTransition: true, v7_relativeSplatPath: true}}>
      <ErrorBoundary
        FallbackComponent={ErrorBoundaryFallback}
        onReset={() => {
          clearRelevantQueries(queryClient);
        }}
      >
        <Suspense fallback={<Loader />}>
          <QueryClientProvider client={queryClient}>
            <AppRoutes />
          </QueryClientProvider>
        </Suspense>
      </ErrorBoundary>
    </BrowserRouter>
  );
}

App.displayName = 'AppWrapper';
