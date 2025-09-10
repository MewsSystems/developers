import { useNavigate, useParams } from '@tanstack/react-router';
import { useQuery } from '@tanstack/react-query';
import { LoaderPinwheel } from 'lucide-react';
import { MovieDetails, getMovieDetails } from '@/entities/movie';

/**
 * Movie details page component
 */
export function MovieDetailsPage() {
  const { movieId } = useParams({ from: '/movie/$movieId' });
  const navigate = useNavigate();
  const id = parseInt(movieId, 10);

  const { data: movie, isLoading, error } = useQuery({
    queryKey: ['movie', id],
    queryFn: () => getMovieDetails(id),
    enabled: !isNaN(id),
    staleTime: 10 * 60 * 1000, // 10 minutes
    gcTime: 30 * 60 * 1000, // 30 minutes
  });

    if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-[500px]">
        <LoaderPinwheel className="h-6 w-6 text-blue-500 animate-spin" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center p-4 bg-red-50 text-red-600 rounded-lg">
        <p>An error occurred while loading movie information</p>
        <p className="text-sm">{error.message}</p>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <button
        onClick={() => navigate({ to: '/' })}
        className="mb-6 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
      >
        ← Back to Movies
      </button>
      <MovieDetails
        movie={movie || null}
        isLoading={isLoading}
        error={error}
      />
    </div>
  );
}