import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from '@/components/ui/card'
import { Star, Calendar } from 'lucide-react'

import { type MovieResult } from '@/schemas/movie'
import { formatDate, getGenres, getRatingColor } from '@/lib/utils'
import { useNavigate } from '@tanstack/react-router'
import { ROUTES } from '@/lib/constants'
import { MoviePoster } from '@/components/MoviePoster'

type Props = {
  movie: MovieResult
}

export const MovieCard = ({ movie }: Props) => {
  const navigate = useNavigate()
  const goToMovieDetail = () =>
    navigate({
      to: ROUTES.MOVIE_DETAIL(movie.id),
    })

  const ratingColorClass = getRatingColor(movie.vote_average)

  return (
    <Card
      key={movie.id}
      className="overflow-hidden flex flex-col h-full pt-0 cursor-pointer"
      onClick={goToMovieDetail}
    >
      <div className="h-64 relative overflow-hidden">
        <MoviePoster
          posterPath={movie.poster_path}
          alt={movie.title}
          className="w-full h-full object-cover transition-transform hover:scale-105"
        />
      </div>

      <CardHeader>
        <CardTitle className="line-clamp-1">{movie.title}</CardTitle>
        <CardDescription className="flex items-center gap-1">
          <Star className="h-4 w-4 text- text-yellow-500" />
          <span className={ratingColorClass}>
            {movie.vote_average.toFixed(1)}
          </span>
          <span className="text-gray-400 text-xs">
            ({movie.vote_count} {movie.vote_count === 1 ? 'vote' : 'votes'})
          </span>
        </CardDescription>
      </CardHeader>

      <CardContent className="flex-grow">
        <p className="text-sm text-gray-500 mb-2 flex items-center gap-1">
          <Calendar className="h-4 w-4" />
          {formatDate(movie.release_date)}
        </p>
        {movie.genre_ids && movie.genre_ids.length > 0 && (
          <p className="text-xs text-gray-400 mb-3">
            {getGenres(movie.genre_ids)}
          </p>
        )}

        <p className="text-sm line-clamp-3">{movie.overview}</p>
      </CardContent>
    </Card>
  )
}
