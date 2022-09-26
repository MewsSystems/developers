import { FC } from "react";

import type { MovieDetail } from "~/features/movieDetail/types";
import { MovieDetailGenres } from "../MovieDetailGenres";
import { MovieDetailDescriptionContainer } from "./styled";

type Props = {
  movieDetailDescription: Omit<MovieDetail, "poster_path">;
};

export const MovieDetailDescription: FC<Props> = ({
  movieDetailDescription,
}) => {
  const { original_title, overview, genres } = movieDetailDescription;

  return (
    <MovieDetailDescriptionContainer>
      <h1>{original_title || ""}</h1>
      <div>
        <MovieDetailGenres genres={genres} />
      </div>
      <div>
        <h2>Overview</h2>
        <p>{overview || "There is no overview at the mooment."}</p>
      </div>
    </MovieDetailDescriptionContainer>
  );
};
