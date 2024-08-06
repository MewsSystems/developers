import React from "react";
import { Link } from "react-router-dom";
import styled from "styled-components";
import { getMovieImageUrl } from "../api/tmdb";
import { Movie } from "../types/MovieInterfaces";

const Card = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const Image = styled.img`
  width: 100%;
  aspect-ratio: 9/13;
  object-fit: cover;
  border: 2px solid #e7e7e7;
  border-radius: 15px;
`;

const StyledLink = styled(Link)`
  text-decoration: none;
  color: inherit;
`;

const Title = styled.h3`
  font-size: 1.2rem;
  margin: 0.5rem 0;
`;

interface MovieCardProps {
  movie: Movie;
}

const MovieCard: React.FC<MovieCardProps> = ({ movie }) => {
  return (
    <Card>
      <StyledLink to={`/movie/${movie.id}`}>
        <Image src={getMovieImageUrl(movie.poster_path)} alt={movie.title} />
        <Title>
          {movie.title}
          {movie.release_date && ` (${movie.release_date.split("-")[0]})`}
        </Title>
        <p>Rating: {Number(movie.vote_average).toFixed(1)}</p>
      </StyledLink>
    </Card>
  );
};

export default MovieCard;
