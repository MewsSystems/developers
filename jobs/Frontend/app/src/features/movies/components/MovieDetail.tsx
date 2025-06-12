import { FC } from 'react';
import styled from 'styled-components';
import { Movie } from '../types';

const Container = styled.div`
  max-width: 800px;
  width: 100%;
  margin: 0 auto;
  background: white;
  border-radius: 10px;
  padding: 2rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
`;

const Title = styled.h1`
  font-size: 2rem;
  margin-bottom: 1rem;
`;

const Poster = styled.img`
  width: 100%;
  max-width: 300px;
  border-radius: 10px;
  margin-bottom: 1rem;
`;

const Info = styled.div`
  margin-top: 1rem;
`;

const Text = styled.p`
  line-height: 1.5;
`;

type Props = {
  movie: Movie;
};

const MovieDetail: FC<Props> = ({ movie }) => {
  const { title, poster_path, release_date, vote_average, overview } = movie;
  return (
    <Container>
      <Title>{title}</Title>
      <Poster
        src={
          poster_path
            ? `https://image.tmdb.org/t/p/w500${poster_path}`
            : 'https://via.placeholder.com/500x750?text=No+Image'
        }
        alt={title}
      />
      <Info>
        <Text>
          <strong>Release Date:</strong> {release_date}
        </Text>
        <Text>
          <strong>Rating:</strong> {vote_average}/10
        </Text>
        <Text>
          <strong>Overview:</strong> {overview}
        </Text>
      </Info>
    </Container>
  );
};

export default MovieDetail;
