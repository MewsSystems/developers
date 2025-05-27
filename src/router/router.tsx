import { lazy } from 'react';
import { RouteObject } from 'react-router-dom';

const Home = lazy(() => import('../pages/home/home').then((module) => ({ default: module.Home })));
const MovieDetail = lazy(() =>
  import('../pages/movie-detail/movie-detail').then((module) => ({ default: module.MovieDetail }))
);
const NotFound = lazy(() =>
  import('../pages/not-found/not-found').then((module) => ({ default: module.NotFound }))
);

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <Home />,
  },
  {
    path: '/movie/:id',
    element: <MovieDetail />,
  },
  {
    path: '*',
    element: <NotFound />,
  },
];
