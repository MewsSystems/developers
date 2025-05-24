import { useParams, useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { getMovie } from '@movie/services/api/movie-api'
import { useService } from '@app/lib/useService';

const Container = styled.div`
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
`;

const BackButton = styled.button`
  background: none;
  border: none;
  color: #666;
  cursor: pointer;
  font-size: 1rem;
  padding: 0.5rem 0;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;

  &:hover {
    color: #333;
  }
`;

const MovieGrid = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 2rem;
`;

const Poster = styled.img`
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
`;

const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;

const Title = styled.h1`
  margin: 0;
  font-size: 2rem;
  color: #333;
`;

const ReleaseDate = styled.p`
  color: #666;
  margin: 0;
`;

const Overview = styled.p`
  color: #444;
  line-height: 1.6;
  margin: 0;
`;

const Rating = styled.div`
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #666;
`;

export const MovieDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: movie, loading, error } = useService(() => getMovie(Number(id)));

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  if (!movie) return <div>Movie not found</div>;
  

  return (
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
  );
}; 