import { MovieResult } from "@/api/schemas/movieSchema";
import { NavLink } from "react-router";

interface Props {
  movie: MovieResult;
}

const MovieCard = ({ movie }: Props) => {
  return (
    <NavLink to={`/movie/${movie.id}`}>
      <div className="shadow-md p-4 rounded-lg bg-amber-300">
        <div className="font-bold">{movie.title}</div>
        <div>{movie.year}</div>
        <div>{movie.overview}</div>
        <div>{movie.voteAverage}</div>
        <img src={movie.image} alt={movie.title} />
      </div>
    </NavLink>
  );
};

export default MovieCard;