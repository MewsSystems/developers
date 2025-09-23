import { Link } from "react-router-dom";
import {
  MovieCardWrapper,
  MovieCardTitle,
  MovieRating,
} from "../styles/styles";
import PosterImage from "./PosterImage";
import Placeholder from "./Placeholder";

type MovieCardProps = {
  id: number;
  title: string;
  poster_path?: string | null;
  vote_average: number;
};

const MovieCard = ({
  id,
  title,
  poster_path,
  vote_average,
}: MovieCardProps) => {
  return (
    <Link
      to={`/movie/${id}`}
      style={{ textDecoration: "none", color: "inherit" }}
    >
      <MovieCardWrapper>
        {poster_path ? (
          <PosterImage posterPath={poster_path} alt={title} maxWidth="200px" />
        ) : (
          <Placeholder width="200px" height="300px" />
        )}
        <MovieCardTitle>{title}</MovieCardTitle>
        <MovieRating>{`${vote_average.toFixed(1)}`}</MovieRating>
      </MovieCardWrapper>
    </Link>
  );
};

export default MovieCard;
