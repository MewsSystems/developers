import { Film, Star } from "lucide-react"
import { Link } from "react-router"
import styled from "styled-components"
import { buildMovieDetailRoute } from "../constants/routes"
import type { Movie } from "../types/movie"
import { getImageUrl, getYear } from "../utils/movieUtils"

const Card = styled(Link)`
  background-color: ${({ theme }) => theme.colors.surface};
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  overflow: hidden;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  text-decoration: none;
  color: inherit;

  &:hover {
    transform: translateY(-4px);
    box-shadow: ${({ theme }) => theme.shadows.lg};
    background-color: ${({ theme }) => theme.colors.surfaceHover};
  }
`

const PosterContainer = styled.div`
  position: relative;
  aspect-ratio: 2/3;
  overflow: hidden;
`

const Poster = styled.img`
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.2s ease;

  ${Card}:hover & {
    transform: scale(1.05);
  }
`

const PosterPlaceholder = styled.div`
  width: 100%;
  height: 100%;
  background-color: ${({ theme }) => theme.colors.border};
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: ${({ theme }) => theme.fontSizes["2xl"]};
  color: ${({ theme }) => theme.colors.textMuted};
`

const CardContent = styled.div`
  padding: ${({ theme }) => theme.spacing.md};
`

const Title = styled.h3`
  font-size: ${({ theme }) => theme.fontSizes.lg};
  margin-bottom: ${({ theme }) => theme.spacing.sm};
  color: ${({ theme }) => theme.colors.text};
  line-height: 1.3;
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
`

const Overview = styled.p`
  color: ${({ theme }) => theme.colors.textSecondary};
  font-size: ${({ theme }) => theme.fontSizes.sm};
  line-height: 1.4;
  margin-bottom: ${({ theme }) => theme.spacing.sm};
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
`

const MovieInfo = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: ${({ theme }) => theme.fontSizes.sm};
  color: ${({ theme }) => theme.colors.textMuted};
`

const Rating = styled.span`
  color: ${({ theme }) => theme.colors.primary};
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 4px;
`

const Year = styled.span``

interface MovieCardProps {
  movie: Movie
}

export const MovieCard = ({ movie }: MovieCardProps) => {
  return (
    <Card to={buildMovieDetailRoute(movie.id)}>
      <PosterContainer>
        {movie.poster_path ? (
          <Poster src={getImageUrl(movie.poster_path) || ""} alt={movie.title} loading="lazy" />
        ) : (
          <PosterPlaceholder>
            <Film size={48} />
          </PosterPlaceholder>
        )}
      </PosterContainer>

      <CardContent>
        <Title>{movie.title}</Title>
        {movie.overview && <Overview>{movie.overview}</Overview>}
        <MovieInfo>
          <Rating>
            <Star size={16} fill="currentColor" />
            {movie.vote_average.toFixed(1)}
          </Rating>
          <Year>{getYear(movie.release_date)}</Year>
        </MovieInfo>
      </CardContent>
    </Card>
  )
}
