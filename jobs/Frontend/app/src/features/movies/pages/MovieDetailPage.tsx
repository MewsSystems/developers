import { FC } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { useMovieDetail } from '../hooks';
import { MovieDetail } from '../components';

const Wrapper = styled.div`
  padding: 2rem;
  display: flex;
  justify-content: center;
  flex-direction: column;
`;

const Button = styled.button`
  margin: 2rem auto 2rem 0;
  padding: 0.5rem 1rem;
  font-size: 1rem;
  cursor: pointer;
`;

const MovieDetailPage: FC = () => {
  const { id } = useParams();
  const movieId = parseInt(id || '', 10);
  const { data, isLoading, isError } = useMovieDetail(movieId);
  const navigate = useNavigate();
  return (
    <Wrapper>
      <Button onClick={() => navigate(-1)}>Back to Search</Button>
      {isLoading && <div>Loading...</div>}
      {isError && <div>Failed to load movie details.</div>}
      {data && <MovieDetail movie={data} />}
    </Wrapper>
  );
};

export default MovieDetailPage;
