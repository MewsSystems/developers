import { LoaderCircleIcon } from 'lucide-react';
import type { MovieDetails as MovieDetailsType } from '../types';

interface MovieDetailsProps {
  movie: MovieDetailsType | null;
  isLoading: boolean;
  error: Error | null;
}

/**
 * Component for displaying detailed movie information
 */
export const MovieDetails = ({ movie, isLoading, error }: MovieDetailsProps) => {
  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-[500px]">
        <LoaderCircleIcon className="h-6 w-6 text-blue-500 animate-spin" />
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

  if (!movie) {
    return (
      <div className="text-center p-8 bg-gray-50 rounded-lg">
        <p className="text-gray-600">Movie information not found</p>
      </div>
    );
  }

  const backdropUrl = movie.backdrop_path
    ? `https://image.tmdb.org/t/p/w1280${movie.backdrop_path}`
    : null;

  const posterUrl = movie.poster_path
    ? `https://image.tmdb.org/t/p/w500${movie.poster_path}`
    : 'https://via.placeholder.com/500x750?text=No+Poster';

  // Format release date
  const formatReleaseDate = (dateString: string) => {
    if (!dateString) return 'Unknown';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  // Format budget and revenue
  const formatCurrency = (amount: number) => {
    if (!amount) return 'Unknown';
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      maximumFractionDigits: 0,
    }).format(amount);
  };

  return (
    <div className="bg-white rounded-lg shadow-lg overflow-hidden">
      {/* Background image */}
      {backdropUrl && (
        <div className="relative h-64 md:h-96 w-full">
          <div
            className="absolute inset-0 bg-cover bg-center"
            style={{ backgroundImage: `url(${backdropUrl})` }}
          ></div>
        </div>
      )}

      <div className="container mx-auto px-4 py-8">
        <div className="flex flex-col md:flex-row -mx-4">
          {/* Poster */}
          <div className="md:flex-shrink-0 px-4 mb-6 md:mb-0">
            <img
              src={posterUrl}
              alt={`${movie.title} poster`}
              className="rounded-lg shadow-md w-full md:w-64 lg:w-80 mx-auto md:mx-0"
            />
          </div>

          {/* Movie information */}
          <div className="px-4">
            <h1 className="text-3xl md:text-4xl font-bold mb-2">{movie.title}</h1>
            
            {movie.tagline && (
              <p className="text-gray-500 italic mb-4">{movie.tagline}</p>
            )}

            <div className="flex flex-wrap items-center mb-4">
              {movie.release_date && (
                <span className="bg-blue-100 text-blue-800 text-sm font-medium mr-2 px-2.5 py-0.5 rounded">
                  {new Date(movie.release_date).getFullYear()}
                </span>
              )}
              
              {movie.runtime && (
                <span className="bg-gray-100 text-gray-800 text-sm font-medium mr-2 px-2.5 py-0.5 rounded">
                  {Math.floor(movie.runtime / 60)}h {movie.runtime % 60}min
                </span>
              )}
              
              {movie.vote_average > 0 && (
                <span className="bg-yellow-100 text-yellow-800 text-sm font-medium mr-2 px-2.5 py-0.5 rounded flex items-center">
                  <svg className="w-4 h-4 mr-1 text-yellow-500" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
                    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"></path>
                  </svg>
                  {movie.vote_average.toFixed(1)}
                </span>
              )}
            </div>

            {/* Genres */}
              <div className="mb-4">
                <div className="flex flex-wrap gap-2">
                  {movie.genres.map((genre) => (
                    <span
                      key={genre.id}
                      className="bg-gray-200 text-gray-700 px-3 py-1 rounded-full text-sm"
                    >
                      {genre.name}
                    </span>
                  ))}
                </div>
              </div>

            {/* Overview */}
            {movie.overview && (
              <div className="mb-6">
                <h2 className="text-xl font-semibold mb-2">Overview</h2>
                <p className="text-gray-700">{movie.overview}</p>
              </div>
            )}

            {/* Additional information */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
              <div>
                <h2 className="text-xl font-semibold mb-2">Information</h2>
                <ul className="space-y-2">
                  <li>
                    <span className="font-medium">Status:</span>{' '}
                    {movie.status || 'Unknown'}
                  </li>
                  <li>
                    <span className="font-medium">Release Date:</span>{' '}
                    {formatReleaseDate(movie.release_date)}
                  </li>
                  <li>
                    <span className="font-medium">Budget:</span>{' '}
                    {formatCurrency(movie.budget)}
                  </li>
                  <li>
                    <span className="font-medium">Revenue:</span>{' '}
                    {formatCurrency(movie.revenue)}
                  </li>
                </ul>
              </div>

              {/* Production companies */}
                <div>
                  <h2 className="text-xl font-semibold mb-2">Production</h2>
                  <ul className="space-y-1">
                    {movie.production_companies.map((company) => (
                      <li key={company.id}>{company.name}</li>
                    ))}
                  </ul>
                </div>
            </div>

            {/* TMDB Attribution */}
            <div className="mt-8 text-sm text-gray-500">
              <p>
                Data provided by{' '}
                <a
                  href="https://www.themoviedb.org/"
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-600 hover:underline"
                >
                  The Movie Database (TMDB)
                </a>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};