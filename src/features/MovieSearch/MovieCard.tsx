import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from '@/components/ui/card'
import { Star, Calendar, Film } from 'lucide-react'

import { type MovieResult } from '@/schemas/movie'
import { formatDate, getGenres } from '@/lib/utils'
import { useNavigate } from '@tanstack/react-router'

type Props = {
  movie: MovieResult
}

const IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/w500'

export const MovieCard = ({ movie }: Props) => {
  const navigate = useNavigate()
  const goToMovieDetail = () =>
    navigate({
      to: `/movie/${movie.id}`,
    })
  return (
    <Card
      key={movie.id}
      className="overflow-hidden flex flex-col h-full pt-0 cursor-pointer"
      onClick={goToMovieDetail}
    >
      <div className="h-64 relative overflow-hidden">
        {movie.poster_path ? (
          <img
            src={`${IMAGE_BASE_URL}${movie.poster_path}`}
            alt={movie.title}
            className="w-full h-full object-cover transition-transform hover:scale-105"
          />
        ) : (
          <div className="w-full h-full flex items-center justify-center bg-gray-100">
            <Film size={48} className="text-gray-400" />
          </div>
        )}
      </div>

      <CardHeader>
        <CardTitle className="line-clamp-1">{movie.title}</CardTitle>
        <CardDescription className="flex items-center gap-1">
          <Star className="h-4 w-4 text-yellow-500" />
          <span>{movie.vote_average.toFixed(1)}</span>
          <span className="text-gray-400 text-xs">
            ({movie.vote_count} votes)
          </span>
        </CardDescription>
      </CardHeader>

      <CardContent className="flex-grow">
        {movie.release_date && (
          <p className="text-sm text-gray-500 mb-2 flex items-center gap-1">
            <Calendar className="h-4 w-4" />
            {formatDate(movie.release_date)}
          </p>
        )}
        <p className="text-xs text-gray-400 mb-3">
          {getGenres(movie.genre_ids)}
        </p>
        <p className="text-sm line-clamp-3">{movie.overview}</p>
      </CardContent>

      {/* <CardFooter className="pt-0">
          <Button
            variant="outline"
            className="w-full"
            onClick={() => alert(`View details for ${movie.title}`)}
          >
            View Details
          </Button>
        </CardFooter> */}
    </Card>
  )
}
