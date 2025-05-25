import { useParams, useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { getMovie } from '@movie/services/api/movie-api'
import { useService } from '@app/lib/use-service';
import { ErrorComponent } from '@core/error/components/error-component';
import { Img } from '@app/lib/components/image/image';
import { MovieDetailSkeleton } from '@app/lib/components/skeleton-movie-detail/skeleton-movie-detail';

const BackdropContainer = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: -1;
  overflow: hidden;

  &::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.3);
  }
`;

const BackdropImage = styled(Img)`
  width: 100%;
  height: 100%;
  object-fit: cover;
  filter: blur(8px);
  transform: scale(1.1);
`;

const Container = styled.div`
  min-height: 100vh;
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
  position: relative;
  z-index: 1;
  color: white;
`;

const BackButton = styled.button`
  background: none;
  border: none;
  color: white;
  cursor: pointer;
  font-size: 1rem;
  padding: 0.5rem 0;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  opacity: 0.8;
  transition: opacity 0.3s ease;

  &:hover {
    opacity: 1;
  }
`;

const MovieGrid = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 2rem;
  background: rgba(40, 40, 40, 0.7);
  padding: 2rem;
  border-radius: 12px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
`;

const Poster = styled.img`
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
`;

const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;

const Title = styled.h1`
  margin: 0;
  font-size: 2.5rem;
  color: white;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
`;

const ReleaseDate = styled.p`
  color: rgba(255, 255, 255, 0.9);
  margin: 0;
  font-size: 1.1rem;
`;

const Overview = styled.p`
  color: rgba(255, 255, 255, 0.9);
  line-height: 1.6;
  margin: 0;
  font-size: 1.1rem;
`;

const Rating = styled.div`
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: rgba(255, 255, 255, 0.9);
  font-size: 1.1rem;
`;

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