import { FC, PropsWithChildren } from 'react';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import MovieDetails from '../../movieDetails/MovieDetails';
import Movies from '../../movies/Movies';
import AppRoutes from '../../shared/enums/AppRoutes';

const AppRouterProvider: FC<PropsWithChildren> = () => {
    return <RouterProvider router={router} />;
};

export default AppRouterProvider;

const router = createBrowserRouter([
    {
        path: AppRoutes.Movies,
        element: <Movies />,
    },
    {
        path: AppRoutes.MovieDetails,
        element: <MovieDetails />,
    },
]);
