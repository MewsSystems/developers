import { ArrowLeft, Film, Star } from "lucide-react"
import { useEffect } from "react"
import { useLocation, useNavigate, useParams } from "react-router"
import { ErrorMessage } from "../../components/ErrorMessage/ErrorMessage"
import { MovieDetailSkeleton } from "../../components/MovieDetailSkeleton/MovieDetailSkeleton"
import { useMovieDetails } from "../../hooks/useMovies"
import { formatNumber, formatRuntime, getImageUrl, getYear } from "../../utils/movieUtils"
import {
  BackButton,
  Container,
  GenreList,
  GenreTag,
  MetaInfo,
  MovieHeader,
  MovieInfo,
  Overview,
  Poster,
  PosterContainer,
  PosterPlaceholder,
  ProductionCompany,
  ProductionList,
  ProductionSection,
  Rating,
  SectionTitle,
  Title,
} from "./MovieDetailPage.styles"

const MovieDetailPage = () => {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const location = useLocation()
  const movieId = Number(id) || 0

  const { data: movie, isLoading, error } = useMovieDetails(movieId)

  useEffect(() => {
    const state = location.state as { scrollToTop?: boolean } | null
    if (state?.scrollToTop && !isLoading && movie) {
      window.scrollTo({ top: 0, behavior: "smooth" })
    }
  }, [location.state, isLoading, movie])

  const handleBackClick = () => {
    navigate(-1)
  }

  if (isLoading) {
    return <MovieDetailSkeleton />
  }

  if (error || !movie) {
    return (
      <Container>
        <BackButton onClick={handleBackClick}>
          <ArrowLeft size={16} />
          Back to Search
        </BackButton>
        <ErrorMessage message="Failed to load movie details. Please try again." />
      </Container>
    )
  }

  return (
    <Container>
      <BackButton onClick={handleBackClick}>
        <ArrowLeft size={16} />
        Back to Search
      </BackButton>

      <MovieHeader>
        <PosterContainer>
          {movie.poster_path ? (
            <Poster src={getImageUrl(movie.poster_path) || ""} alt={movie.title} loading="lazy" />
          ) : (
            <PosterPlaceholder>
              <Film size={64} />
            </PosterPlaceholder>
          )}
        </PosterContainer>

        <MovieInfo>
          <Title>{movie.title}</Title>

          <MetaInfo>
            <Rating data-testid="movie-rating">
              <Star size={20} fill="currentColor" />
              {movie.vote_average.toFixed(1)}
            </Rating>
            <span>{getYear(movie.release_date)}</span>
            {movie.runtime && <span>{formatRuntime(movie.runtime)}</span>}
            <span>{formatNumber(movie.vote_count)} votes</span>
          </MetaInfo>

          {movie.genres && movie.genres.length > 0 && (
            <GenreList>
              {movie.genres.map((genre) => (
                <GenreTag key={genre.id}>{genre.name}</GenreTag>
              ))}
            </GenreList>
          )}

          {movie.overview && <Overview>{movie.overview}</Overview>}
        </MovieInfo>
      </MovieHeader>

      {movie.production_companies && movie.production_companies.length > 0 && (
        <ProductionSection>
          <SectionTitle>Production Companies</SectionTitle>
          <ProductionList>
            {movie.production_companies.map((company) => (
              <ProductionCompany key={company.id}>{company.name}</ProductionCompany>
            ))}
          </ProductionList>
        </ProductionSection>
      )}
    </Container>
  )
}

export default MovieDetailPage
