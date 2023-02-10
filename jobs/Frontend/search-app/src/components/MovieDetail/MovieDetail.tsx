import { useAppSelector } from "../../app/hooks";
import { Image } from "../Image/Image";

import { DetailHeader } from "./DetailHeader";
import { MovieCategories } from "./MovieCategories";
import { Ratings } from "../Ratings/Ratings";
import { selectMoviesState } from "../../selectors/movies";
import { Detail, Overview, SubTitle, Scores } from "./MovieDetail.styled";

export const MovieDetail = () => {
  const { movieDetail } = useAppSelector(selectMoviesState);

  if (!movieDetail) {
    return null;
  }

  return (
    <>
      <DetailHeader movieDetail={movieDetail} />

      <Detail>
        {movieDetail.poster_path && (
          <Image
            src={movieDetail.poster_path}
            alt={movieDetail.title}
            size={250}
          />
        )}
        <div>
          <MovieCategories movieDetail={movieDetail} />
          <SubTitle data-test="subtitle">{movieDetail.tagline}</SubTitle>
          <Overview data-test="overview">{movieDetail.overview}</Overview>
          <Scores>
            <Ratings movie={movieDetail} />
          </Scores>
        </div>
      </Detail>
    </>
  );
};
