import { MovieResult } from "@/api/schemas/movieSchema";
import { NavLink } from "react-router";
import YearAndRating from "../../../components/YearAndRating";

interface Props {
  movie: MovieResult;
}

const MovieCard = ({ movie }: Props) => (
  <NavLink to={`/movie/${movie.id}`}>
    <div className="rounded-lg border border-border hover:ring-2 ring-ring-muted h-80">
      <div className="rounded-lg rounded-b-none overflow-hidden relative w-full h-40">
        <img className="absolute w-full h-full object-cover"
          src={movie.image}
          alt={movie.title} />
        <div className="absolute bottom-0 w-full h-2/3 bg-gradient-to-t from-background to-transparent" />
      </div>
      <div className="p-4 mt-2">
        <div className="font-bold text-md line-clamp-1">{movie.title}</div>
        <YearAndRating year={movie.year} rating={movie.voteAverage} ratingCount={movie.voteCount} />
        <p className="mt-4 text-foreground-secondary line-clamp-2">
          {movie.overview}
        </p>
      </div>
    </div>
  </NavLink>
);

export default MovieCard;
