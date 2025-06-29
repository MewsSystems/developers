import {
  MovieDetailsRow,
  MovieDetailsTitle,
  MovieOverview,
  MovieDetailsList,
  MovieDetailsBadge,
} from "./DetailPage.internal";
import type { MovieDetailsResponse } from "../../api/types";

interface MovieContentProps {
  movie: MovieDetailsResponse;
}

export const MovieContent = (props: MovieContentProps) => {
  return (
    <>
      <MovieDetailsRow>
        <MovieDetailsTitle>Overview</MovieDetailsTitle>
        <MovieOverview>{props.movie.overview}</MovieOverview>
      </MovieDetailsRow>
      <MovieDetailsRow>
        {props.movie.production_companies.length > 0 ? (
          <>
            <MovieDetailsTitle>Production</MovieDetailsTitle>
            <MovieDetailsList>
              {props.movie.production_companies.map((company) => (
                <MovieDetailsBadge $isInverted key={company.id}>
                  <span>{company.name}</span>
                </MovieDetailsBadge>
              ))}
            </MovieDetailsList>
          </>
        ) : null}
      </MovieDetailsRow>
    </>
  );
};
