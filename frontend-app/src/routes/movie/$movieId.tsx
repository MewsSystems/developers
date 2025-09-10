import { createFileRoute } from '@tanstack/react-router';
import { MovieDetailsPage } from '../../pages/movie-details';

export const Route = createFileRoute('/movie/$movieId')({ 
  component: MovieDetailsPage
});