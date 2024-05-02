import { Movie } from "src/api/tmdb";
import styled from "styled-components";
import { getFullYear } from "src/utils/date";
import PosterImage from "./PosterImage";

type MovieCardProps = {
  movie: Movie;
};

function MovieCard({ movie }: MovieCardProps) {
  return (
    <Figure>
      <PosterImage path={movie.poster_path} size="w342" />
      <figcaption>
        <Title>{movie.title}</Title>
        <Year>{getFullYear(movie.release_date)}</Year>
      </figcaption>
    </Figure>
  );
}

const Title = styled.h3`
  color: var(--color-dark-text);
  font-size: 0.875rem;
  font-weight: 500;
`;

const Year = styled.p`
  color: var(--color-light-text);
  font-size: 0.7rem;
  font-weight: 500;
`;

const Figure = styled.figure`
  width: 222px;
  max-width: min(342px, 100%);
  cursor: pointer;

  &:active,
  &:hover {
    outline: 2px solid var(--color-light-gray);
    border-radius: 12px;
  }

  &:active {
    outline: 3px solid black;
  }

  figcaption {
    overflow: hidden;
    text-overflow: ellipsis;
    text-align: left;
    padding: 6px 3px;
  }
`;

export default MovieCard;
