import { MovieResult } from "@/api/schemas/movieSchema";

interface Props {
  movie: MovieResult;
}

const MovieCard = ({ movie }: Props) => {
  return (
    <div className="shadow-md p-4 rounded-lg">
      <div className="font-bold">{movie.title}</div>
      <div>{movie.year}</div>
      <div>{movie.overview}</div>
      <div>{movie.voteAverage}</div>
      <img src={movie.image} alt={movie.title} />
    </div>
  );
};

export default MovieCard;