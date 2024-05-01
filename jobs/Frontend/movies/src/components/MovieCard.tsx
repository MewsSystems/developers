import { Movie, posterUrl } from "src/api/tmdb";
import styled from "styled-components";
import FilmIcon from "src/assets/film.svg?component";
import { getFullYear } from "src/utils/date";

type MovieCardProps = {
  movie: Movie;
};

function MovieCard({ movie }: MovieCardProps) {
  return (
    <Figure>
      {movie.poster_path ? (
        <img
          src={posterUrl(movie.poster_path, "w342")}
          alt="poster"
          role="presentation"
        />
      ) : (
        <EmptyImage />
      )}
      <figcaption>
        <Title>{movie.title}</Title>
        <Year>{getFullYear(movie.release_date)}</Year>
      </figcaption>
    </Figure>
  );
}

const EmptyImage = styled(FilmIcon)`
  margin: auto;
  color: white;
  background-color: var(--color-light-gray);
`;

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

  img,
  ${EmptyImage} {
    width: 100%;
    height: 100%;
    border-radius: 12px;
  }

  figcaption {
    overflow: hidden;
    text-overflow: ellipsis;
    text-align: left;
    padding: 6px 3px;
  }
`;

export default MovieCard;
