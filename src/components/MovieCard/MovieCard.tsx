import { Film, Star } from "lucide-react"
import { buildMovieDetailRoute } from "@/constants/routes"
import type { Movie } from "@/types/movie"
import { getImageUrl, getYear } from "@/utils/movieUtils"
import {
  Card,
  CardContent,
  MovieInfo,
  Overview,
  Poster,
  PosterContainer,
  PosterPlaceholder,
  Rating,
  Title,
  Year,
} from "./MovieCard.styles"

export interface MovieCardProps {
  movie: Movie
}

export const MovieCard = ({ movie }: MovieCardProps) => {
  return (
    <Card
      to={buildMovieDetailRoute(movie.id)}
      state={{ scrollToTop: true }}
      data-testid="movie-card"
    >
      <PosterContainer>
        {movie.poster_path ? (
          <Poster src={getImageUrl(movie.poster_path) || ""} alt={movie.title} loading="lazy" />
        ) : (
          <PosterPlaceholder>
            <Film size={24} />
          </PosterPlaceholder>
        )}
      </PosterContainer>

      <CardContent>
        <Title>{movie.title}</Title>
        {movie.overview && <Overview>{movie.overview}</Overview>}
        <MovieInfo>
          <Rating data-testid="movie-rating">
            <Star size={14} fill="currentColor" />
            {movie.vote_average.toFixed(1)}
          </Rating>
          <Year>{getYear(movie.release_date)}</Year>
        </MovieInfo>
      </CardContent>
    </Card>
  )
}
