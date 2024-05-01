import { Movie } from "src/api/fmdb";
import styled from "styled-components";

type MovieCardProps = {
  movie: Movie;
};

function getFullYear(date: string) {
  return new Date(date).getFullYear();
}

function MovieCard({ movie }: MovieCardProps) {
  return (
    <Figure>
      <img
        src=""
        alt=""
        role="presentation"
      />
      <figcaption>
        {movie.title} / {movie.original_title} /{" "}
        {getFullYear(movie.release_date)}
      </figcaption>
    </Figure>
  );
}

const Figure = styled.figure`
  max-width: 200px;
  img {
    min-width: 50px;
    min-height: 50px;
    background-color: silver;
    border-radius: 12px;
  }
  figcaption {
    padding: 1rem;
    border: 3px dashed coral;
    border-radius: 12px;
  }
`;

export default MovieCard;
