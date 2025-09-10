import { Link } from '@tanstack/react-router';
import type { Movie } from '../types';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/shared/ui/card';

interface MovieCardProps {
  movie: Movie;
}

/**
 * Modern movie card component with extended information
 * Uses shadcn/ui components for consistent design
 */
export const MovieCard = ({ movie }: MovieCardProps) => {
  const posterUrl = movie.poster_path
    ? `https://image.tmdb.org/t/p/w500${movie.poster_path}`
    : '/placeholder-poster.png';

  // Format release date
  const releaseYear = movie.release_date 
    ? new Date(movie.release_date).getFullYear() 
    : 'N/A';

  // Format rating
  const rating = movie.vote_average ? movie.vote_average.toFixed(1) : 'N/A';

  // Truncate overview for preview
  const truncatedOverview = movie.overview 
    ? movie.overview.length > 120 
      ? `${movie.overview.substring(0, 120)}...` 
      : movie.overview
    : 'Description not available';

  // Determine rating color based on value
  const getRatingColor = () => {
    if (typeof rating !== 'number') return 'text-red-600';
    if (rating >= 8) return 'text-green-600';
    if (rating >= 6) return 'text-yellow-600';
    if (rating >= 4) return 'text-orange-600';
    return 'text-red-600';
  };

  return (
    <Card className="group overflow-hidden transition-all duration-300 hover:shadow-xl hover:-translate-y-1 bg-gradient-to-br from-card to-card/95 border-border/50 pt-0">
      <Link
        to="/movie/$movieId"
        params={{ movieId: movie.id.toString() }}
        className="block h-full"
      >
        {/* Movie poster */}
        <div className="relative overflow-hidden">
          <div className="aspect-[2/3] relative">
            <img
              src={posterUrl}
              alt={movie.title}
              className="absolute inset-0 w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
              loading="lazy"
            />
            {/* Gradient overlay */}
            <div className="absolute inset-0 bg-gradient-to-t from-black/60 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
            

          </div>
        </div>

        {/* Movie information */}
        <CardHeader className="py-3">
          <CardTitle className="text-lg font-bold leading-tight line-clamp-2 group-hover:text-primary transition-colors flex items-center justify-between">
          <div>
            <div>
              {movie.title}
            </div>
            <div className="text-sm text-muted-foreground">
              <span className="font-medium">{releaseYear}</span>
            </div>
          </div>
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <svg
                className="w-4 h-4 text-yellow-400 fill-current"
                viewBox="0 0 20 20"
              >
                <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
              </svg>
              <span className={`text-sm font-semibold ${getRatingColor()}`}>
                {rating}
              </span>
            </div>
          </div>
          </CardTitle>
        </CardHeader>

        <CardContent className="pt-0">
          <CardDescription className="text-sm leading-relaxed line-clamp-3">
            {truncatedOverview}
          </CardDescription>
        </CardContent>
      </Link>
    </Card>
  );
};