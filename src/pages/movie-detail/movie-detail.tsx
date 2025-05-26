import { useParams, useNavigate } from 'react-router-dom';
import { getMovie } from '@movie/services/api/movie-api'
import { useService } from '@app/lib/use-service';
import { ErrorComponent } from '@core/error/components/error-component';
import { MovieDetailSkeleton } from '@app/lib/components/skeleton-movie-detail/skeleton-movie-detail';
import { BackdropContainer, BackdropImage, Container, BackButton, MovieGrid } from './movie-detail.styled';
import { MovieContent } from './components/movie-content/movie-content';

export const MovieDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: movie, loading, error } = useService(() => getMovie(Number(id)));

  if (loading) return <MovieDetailSkeleton />;
  if (error) return <ErrorComponent code={error.code} message={error.message} />;
  if (!movie) return <>Movie not found</>;

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
          <MovieContent movie={movie} />
        </MovieGrid>
      </Container>
    </>
  );
}; 