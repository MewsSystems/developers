import { MovieCategoryCollection, MovieDetailResult } from "../../app/types";
import { Tag } from "../Tag/public-api";

type Props = {
  movieDetail: MovieDetailResult;
};

export const MovieCategories = ({ movieDetail }: Props) => {
  return (
    <>
      {movieDetail.genres?.map((genre: MovieCategoryCollection) => (
        <Tag key={genre.id} value={genre.name} />
      ))}
    </>
  );
};
