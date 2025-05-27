import { Routes, Route } from 'react-router-dom';
import { Suspense } from 'react';
import { MovieCardsSkeleton } from '@app/lib/components/skeleton-cards-list/cards-skeleton-list';
import { routes } from './router/router';

function App() {
  return (
    <Suspense fallback={<MovieCardsSkeleton />}>
      <Routes>
        {routes.map((route) => (
          <Route key={route.path} path={route.path} element={route.element} />
        ))}
      </Routes>
    </Suspense>
  );
}

export default App;
