import { FC } from "react";

import type { Genre } from "~/features/movieDetail/types";
import { GenreList } from "./styled";

type Props = {
  genres?: Genre[];
};

export const MovieDetailGenres: FC<Props> = ({ genres }) => {
  return (
    <GenreList>
      {!!genres?.length &&
        genres.map((genre, index) => {
          const withComa = index + 1 !== genres.length ? ", " : "";
          const genreWithComa = genre?.name ? `${genre?.name}${withComa}` : "";
          return <li key={genre?.id || index}>{genreWithComa}</li>;
        })}
    </GenreList>
  );
};
