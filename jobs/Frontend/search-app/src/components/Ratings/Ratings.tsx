import { MoviesSearchResult } from "../../app/types";
import { Rating } from "./Rating";

type Props = {
  movie: MoviesSearchResult;
};

export const Ratings = ({ movie }: Props) => {
  return (
    <>
      <Rating
        label="Rating"
        isHighlighted={movie.vote_average > 9}
        iconName="fa-solid fa-star">
        <strong>{movie.vote_average?.toFixed(1)}</strong> / 10
      </Rating>

      <Rating
        label="Popularity"
        isHighlighted={movie.popularity > 90}
        iconName="fa-solid fa-fire">
        <strong>{Math.round(movie.popularity)} %</strong>
      </Rating>
    </>
  );
};
