import {Suspense, lazy} from 'react';
import {BrowserRouter} from 'react-router-dom';
import {LoadingSpinner} from './components/LoadingSpinner';
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';

const AppRoutes = lazy(() => import('./navigation/AppRoutes.tsx'));

export default function App() {
  const queryClient = new QueryClient();

  return (
    // pass the "future" prop to suppress warnings from React Router about upcoming changes in v7 in the dev console
    <BrowserRouter future={{v7_startTransition: true, v7_relativeSplatPath: true}}>
      <Suspense fallback={<LoadingSpinner />}>
        <QueryClientProvider client={queryClient}>
          <AppRoutes />
        </QueryClientProvider>
      </Suspense>
    </BrowserRouter>
  );
}

App.displayName = 'AppWrapper';
