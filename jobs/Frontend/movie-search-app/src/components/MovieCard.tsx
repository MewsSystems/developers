import React from 'react';
import { Movie } from '../types';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const Card = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 10px;
  border: 1px solid #ddd;
  margin: 10px;
`;

const Poster = styled.img`
  width: 200px;
  height: 300px;
  object-fit: cover;
`;

const Title = styled.h3`
  font-size: 18px;
  text-align: center;
`;

interface MovieCardProps {
  movie: Movie;
}

const MovieCard: React.FC<MovieCardProps> = ({ movie }) => {
  return (
    <Card>
      <Link to={`/movie/${movie.id}`}>
        <Poster src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`} alt={movie.title} />
        <Title>{movie.title}</Title>
      </Link>
    </Card>
  );
};

export default MovieCard;