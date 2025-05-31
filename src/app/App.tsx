import {Suspense, lazy} from 'react';
import {ErrorBoundary} from 'react-error-boundary';
import {BrowserRouter} from 'react-router-dom';
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import ErrorFallback from './components/ErrorFallback/ErrorFallback';
import Loader from './components/Loader/Loader';

const AppRoutes = lazy(() => import('../routes/AppRoutes'));

export default function App() {
  const queryClient = new QueryClient();

  return (
    // pass the "future" prop to suppress warnings in the browser console from React Router about upcoming changes in v7
    <BrowserRouter future={{v7_startTransition: true, v7_relativeSplatPath: true}}>
      <ErrorBoundary
        FallbackComponent={ErrorFallback}
        onReset={() => {
          queryClient.clear();
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
