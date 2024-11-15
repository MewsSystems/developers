import React from "react";
import { Link } from "react-router-dom";
import styled from "styled-components";
import { getMovieImageUrl } from "../api/tmdb";
import { Movie } from "../types/MovieInterfaces";

const Card = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  border-radius: 15px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
`;

const Image = styled.img`
  width: 100%;
  aspect-ratio: 9/13;
  object-fit: cover;
  border-radius: 15px 15px 0 0;
`;

const StyledLink = styled(Link)`
  text-decoration: none;
  color: inherit;
`;

const TextBlock = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-bottom: 1rem;
`;

const Title = styled.h3`
  font-size: 1.2rem;
  margin: 0.5rem;
`;

interface MovieCardProps {
  movie: Movie;
}

const MovieCard: React.FC<MovieCardProps> = ({ movie }) => {
  return (
    <Card>
      <StyledLink to={`/movie/${movie.id}`}>
        <Image src={getMovieImageUrl(movie.poster_path)} alt={movie.title} />
        <TextBlock>
          <Title>
            {movie.title}
            {movie.release_date &&
              ` (${new Date(movie.release_date).getFullYear()})`}
          </Title>
          <p>Rating: {(movie.vote_average || 0).toFixed(1)}</p>
        </TextBlock>
      </StyledLink>
    </Card>
  );
};

export default MovieCard;
