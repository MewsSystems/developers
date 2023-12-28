import { createBrowserRouter } from 'react-router-dom';
import MovieDetails from '../../movieDetails/MovieDetails';
import Movies from '../../movies/Movies';
import AppRoutes from '../../shared/enums/AppRoutes';

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

export default router;
