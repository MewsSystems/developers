import { Link } from "react-router-dom";
import { MovieCardWrapper, MovieCardTitle } from "../styles/styles";
import PosterImage from "./PosterImage";
import Placeholder from "./Placeholder";

type MovieCardProps = {
  id: number;
  title: string;
  poster_path?: string | null;
};

const MovieCard = ({ id, title, poster_path }: MovieCardProps) => {
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
      </MovieCardWrapper>
    </Link>
  );
};

export default MovieCard;
