import { LoaderPinwheel } from 'lucide-react';
import type {Movie} from '@/entities/movie';
import {  MovieCard } from '@/entities/movie';

interface MoviesListProps {
  movies: Array<Movie>;
  isLoading: boolean;
  error: string | null; 
}

/**
 * Component for displaying movie list
 */
export const MoviesList = ({ movies, isLoading, error }: MoviesListProps) => {
  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-[300px]">
        <LoaderPinwheel className="h-6 w-6 text-blue-500 animate-spin" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center p-4 bg-red-50 text-red-600 rounded-lg">
        <p>An error occurred while loading movies</p>
        <p className="text-sm">{error}</p>
      </div>
    );
  }

  if (movies.length === 0) {
    return (
      <div className="text-center p-8 bg-gray-50 rounded-lg">
        <p className="text-gray-600">No movies found</p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6">
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </div>
  );
};