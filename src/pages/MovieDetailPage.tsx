import { Link, useParams } from "react-router"
import styled from "styled-components"
import { ErrorMessage } from "../components/ErrorMessage"
import { LoadingSpinner } from "../components/LoadingSpinner"
import { useMovieDetails } from "../hooks/useMovies"

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: ${({ theme }) => theme.spacing.xl};
`

const BackButton = styled(Link)`
  display: inline-flex;
  align-items: center;
  gap: ${({ theme }) => theme.spacing.sm};
  padding: ${({ theme }) => theme.spacing.sm} ${({ theme }) => theme.spacing.md};
  background-color: ${({ theme }) => theme.colors.surface};
  color: ${({ theme }) => theme.colors.text};
  border-radius: ${({ theme }) => theme.borderRadius.base};
  text-decoration: none;
  margin-bottom: ${({ theme }) => theme.spacing.xl};
  transition: background-color 0.2s ease;

  &:hover {
    background-color: ${({ theme }) => theme.colors.surfaceHover};
  }
`

const MovieHeader = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: ${({ theme }) => theme.spacing.xl};
  margin-bottom: ${({ theme }) => theme.spacing.xl};

  @media (max-width: ${({ theme }) => theme.breakpoints.md}) {
    grid-template-columns: 1fr;
    gap: ${({ theme }) => theme.spacing.lg};
  }
`

const PosterContainer = styled.div`
  position: relative;
  aspect-ratio: 2/3;
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  overflow: hidden;
  box-shadow: ${({ theme }) => theme.shadows.lg};
`

const Poster = styled.img`
  width: 100%;
  height: 100%;
  object-fit: cover;
`

const PosterPlaceholder = styled.div`
  width: 100%;
  height: 100%;
  background-color: ${({ theme }) => theme.colors.border};
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: ${({ theme }) => theme.fontSizes["4xl"]};
  color: ${({ theme }) => theme.colors.textMuted};
`

const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing.md};
`

const Title = styled.h1`
  font-size: ${({ theme }) => theme.fontSizes["4xl"]};
  color: ${({ theme }) => theme.colors.text};
  margin-bottom: ${({ theme }) => theme.spacing.sm};
`

const MetaInfo = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.lg};
  color: ${({ theme }) => theme.colors.textSecondary};
  font-size: ${({ theme }) => theme.fontSizes.lg};
  margin-bottom: ${({ theme }) => theme.spacing.md};
`

const Rating = styled.span`
  color: ${({ theme }) => theme.colors.primary};
  font-weight: 600;
`

const Overview = styled.p`
  font-size: ${({ theme }) => theme.fontSizes.lg};
  line-height: 1.6;
  color: ${({ theme }) => theme.colors.textSecondary};
  margin-bottom: ${({ theme }) => theme.spacing.lg};
`

const GenreList = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.sm};
  margin-bottom: ${({ theme }) => theme.spacing.lg};
`

const GenreTag = styled.span`
  padding: ${({ theme }) => theme.spacing.sm} ${({ theme }) => theme.spacing.md};
  background-color: ${({ theme }) => theme.colors.surface};
  color: ${({ theme }) => theme.colors.primary};
  border-radius: ${({ theme }) => theme.borderRadius.full};
  font-size: ${({ theme }) => theme.fontSizes.sm};
  font-weight: 500;
`

const ProductionSection = styled.div`
  margin-top: ${({ theme }) => theme.spacing.xl};
`

const SectionTitle = styled.h2`
  font-size: ${({ theme }) => theme.fontSizes["2xl"]};
  color: ${({ theme }) => theme.colors.text};
  margin-bottom: ${({ theme }) => theme.spacing.md};
`

const ProductionList = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.md};
`

const ProductionCompany = styled.span`
  padding: ${({ theme }) => theme.spacing.sm} ${({ theme }) => theme.spacing.md};
  background-color: ${({ theme }) => theme.colors.surface};
  color: ${({ theme }) => theme.colors.textSecondary};
  border-radius: ${({ theme }) => theme.borderRadius.base};
  font-size: ${({ theme }) => theme.fontSizes.sm};
`

export const MovieDetailPage = () => {
  const { id } = useParams<{ id: string }>()
  const movieId = Number(id) || 0

  const { data: movie, isLoading, error } = useMovieDetails(movieId)

  const getImageUrl = (path: string | null) => {
    if (!path) return null
    return `https://image.tmdb.org/t/p/w500${path}`
  }

  const getYear = (releaseDate: string) => {
    return new Date(releaseDate).getFullYear()
  }

  const formatRuntime = (minutes: number) => {
    const hours = Math.floor(minutes / 60)
    const mins = minutes % 60
    return `${hours}h ${mins}m`
  }

  const formatNumber = (num: number) => {
    return num.toLocaleString("en-US")
  }

  if (isLoading) {
    return (
      <Container>
        <LoadingSpinner text="Loading movie details..." />
      </Container>
    )
  }

  if (error || !movie) {
    return (
      <Container>
        <BackButton to="/">← Back to Search</BackButton>
        <ErrorMessage message="Failed to load movie details. Please try again." />
      </Container>
    )
  }

  return (
    <Container>
      <BackButton to="/">← Back to Search</BackButton>

      <MovieHeader>
        <PosterContainer>
          {movie.poster_path ? (
            <Poster src={getImageUrl(movie.poster_path) || ""} alt={movie.title} />
          ) : (
            <PosterPlaceholder>🎬</PosterPlaceholder>
          )}
        </PosterContainer>

        <MovieInfo>
          <Title>{movie.title}</Title>

          <MetaInfo>
            <Rating>⭐ {movie.vote_average.toFixed(1)}</Rating>
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
