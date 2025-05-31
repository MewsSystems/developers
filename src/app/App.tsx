import {Suspense, lazy} from 'react';
import {BrowserRouter} from 'react-router-dom';
import Loader from './components/Loader/Loader.tsx';
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';

const AppRoutes = lazy(() => import('../routes/AppRoutes'));

export default function App() {
  const queryClient = new QueryClient();

  return (
    // pass the "future" prop to suppress warnings in the browser console from React Router about upcoming changes in v7
    <BrowserRouter future={{v7_startTransition: true, v7_relativeSplatPath: true}}>
      <Suspense fallback={<Loader />}>
        <QueryClientProvider client={queryClient}>
          <AppRoutes />
        </QueryClientProvider>
      </Suspense>
    </BrowserRouter>
  );
}

App.displayName = 'AppWrapper';
