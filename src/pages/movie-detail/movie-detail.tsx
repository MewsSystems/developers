import { useParams, useNavigate } from 'react-router-dom';
import { getMovie } from '@movie/services/api/movie-api'
import { useService } from '@app/lib/use-service';
import { ErrorComponent } from '@core/error/components/error-component';
import { MovieDetailSkeleton } from '@app/lib/components/skeleton-movie-detail/skeleton-movie-detail';
import { BackdropContainer, BackdropImage, Container, BackButton, MovieGrid, Poster, MovieInfo, Title, MovieDetails, DetailItem, ReleaseDate, Rating, Overview } from './movie-detail.stiled';


const formatRuntime = (minutes: number): string => {
  const hours = Math.floor(minutes / 60);
  const remainingMinutes = minutes % 60;
  return `${hours}h ${remainingMinutes}m`;
};

const formatPopularity = (popularity: number): string => {
  return popularity.toFixed(1);
};

const formatLanguage = (language: string): string => {
  return language.toUpperCase();
};

export const MovieDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: movie, loading, error } = useService(() => getMovie(Number(id)));

  if (loading) return <MovieDetailSkeleton />;
  if (error) return <ErrorComponent code={error.code} message={error.message} />;
  if (!movie) return <div>Movie not found</div>;

  return (
    <>
      <BackdropContainer>
        <BackdropImage 
          src={`https://image.tmdb.org/t/p/original${movie.backdropPath}`} 
          alt={`${movie.title} backdrop`}
          objectFit="cover"
        />
      </BackdropContainer>
      <Container>
        <BackButton onClick={() => navigate(-1)}>
          ← Back to Movies
        </BackButton>
        <MovieGrid>
          <Poster 
            src={`https://image.tmdb.org/t/p/w500${movie.posterPath}`} 
            alt={movie.title}
          />
          <MovieInfo>
            <Title>{movie.title}</Title>
            <MovieDetails>
              {movie.language && (
                <DetailItem>
                  {formatLanguage(movie.language)}
                </DetailItem>
              )}
              {movie.runtime && (
                <DetailItem>
                  {formatRuntime(movie.runtime)}
                </DetailItem>
              )}
              <DetailItem>
                Popularity: {formatPopularity(movie.popularity)}
              </DetailItem>
            </MovieDetails>
            <ReleaseDate>Release Date: {movie.releaseDate}</ReleaseDate>
            <Rating>
              Rating: {movie.voteAverage.toFixed(1)}/10
            </Rating>
            <Overview>{movie.overview}</Overview>
          </MovieInfo>
        </MovieGrid>
      </Container>
    </>
  );
}; 