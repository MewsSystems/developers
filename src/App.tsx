import {Suspense} from 'react';
import {BrowserRouter} from 'react-router-dom';
import AppRoutes from './AppRoutes';
import {LoadingSpinner} from './components/LoadingSpinner';

export default function App() {
  return (
    <BrowserRouter>
      <Suspense fallback={<LoadingSpinner />}>
        <AppRoutes />
      </Suspense>
    </BrowserRouter>
  );
}

App.displayName = 'AppWrapper';
