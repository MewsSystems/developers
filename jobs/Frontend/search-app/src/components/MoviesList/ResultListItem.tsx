import { Link } from "react-router-dom";

import { MoviesSearchResult } from "../../app/types";
import { Image } from "../Image/Image";
import { Ratings } from "../Ratings/Ratings";
import {
  Item,
  MovieInfo,
  MovieName,
  RatingsStyled,
  Release,
  Row,
} from "./ResultListItem.styled";

type Props = {
  movie: MoviesSearchResult;
};

export const ResultListItem = ({ movie }: Props) => {
  return (
    <Item>
      <Link to={`/movie/${movie.id}`}>
        <Image src={movie.poster_path} alt={movie.title} size={60} />
      </Link>

      <Row>
        <MovieInfo>
          <Link to={`/movie/${movie.id}`}>
            <MovieName
              className={
                movie.vote_average > 9 || movie.popularity > 90
                  ? "highlighted"
                  : ""
              }>
              {movie.title}
            </MovieName>
          </Link>
          <Release data-test="release">
            {movie.release_date?.split("-")[0] ?? movie.release_date}
          </Release>
        </MovieInfo>

        <RatingsStyled>
          <Ratings movie={movie} />
        </RatingsStyled>
      </Row>
    </Item>
  );
};
