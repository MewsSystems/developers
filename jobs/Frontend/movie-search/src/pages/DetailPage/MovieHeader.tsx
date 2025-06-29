import { Heading } from "../../components/Typography/Heading";
import { Clock, ThumbsUp } from "lucide-react";
import {
  MovieDetailsRow,
  MovieDetailsTagline,
  MovieDetailsList,
  MovieDetailsBadge,
  MovieDetailsVote,
} from "./DetailPage.internal";
import {
  getTranslatedTitle,
  getYearFromDate,
  formatRuntime,
} from "../../utils/movieHelpers";
import type { MovieDetailsResponse } from "../../api/types";

interface MovieHeaderProps {
  movie: MovieDetailsResponse;
}

export const MovieHeader = (props: MovieHeaderProps) => {
  const movieTitle = getTranslatedTitle(
    props.movie.original_language === "en",
    props.movie.original_title,
    props.movie.title
  );

  const releaseYear = props.movie.release_date
    ? `(${getYearFromDate(props.movie.release_date)})`
    : "(No release date)";

  const hasGenres = props.movie.genres.length > 0;
  const hasRuntime = props.movie.runtime && props.movie.runtime > 0;

  return (
    <>
      <MovieDetailsRow>
        <Heading>
          {movieTitle} {releaseYear}
        </Heading>
        {props.movie.tagline ? (
          <MovieDetailsTagline>{props.movie.tagline}</MovieDetailsTagline>
        ) : null}
        {hasGenres || hasRuntime ? (
          <MovieDetailsList>
            {hasGenres &&
              props.movie.genres.map((genre) => (
                <MovieDetailsBadge key={genre.id}>
                  <span>{genre.name}</span>
                </MovieDetailsBadge>
              ))}
            {hasRuntime && (
              <MovieDetailsBadge>
                <Clock size={12} color="#333" />
                <span>{formatRuntime(props.movie.runtime!)}</span>
              </MovieDetailsBadge>
            )}
          </MovieDetailsList>
        ) : null}
      </MovieDetailsRow>
      <MovieDetailsRow>
        <MovieDetailsVote>
          <ThumbsUp size={24} color="#333" />
          {props.movie.vote_average.toFixed(1)}
        </MovieDetailsVote>
      </MovieDetailsRow>
    </>
  );
};
