import { FC } from 'react';
import styled from 'styled-components';
import { useNavigate } from 'react-router-dom';
import { Movie } from '../types';

const Card = styled.div`
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  cursor: pointer;
  transition: transform 0.2s ease;

  &:hover {
    transform: scale(1.02);
  }
`;

const Poster = styled.img`
  width: 100%;
  height: 300px;
  object-fit: cover;
`;

const Info = styled.div`
  padding: 0.5rem 1rem;
`;

const Title = styled.h3`
  font-size: 1rem;
  margin: 0.5rem 0;
`;

type Props = {
  movie: Movie;
};

const MovieCard: FC<Props> = ({ movie }) => {
  const navigate = useNavigate();
  const { id, title, poster_path } = movie;
  return (
    <Card onClick={() => navigate(`/movie/${id}`)}>
      <Poster
        src={
          poster_path
            ? `https://image.tmdb.org/t/p/w500${poster_path}`
            : 'https://via.placeholder.com/500x750?text=No+Image'
        }
        alt={title}
      />
      <Info>
        <Title>{title}</Title>
      </Info>
    </Card>
  );
};

export default MovieCard;
