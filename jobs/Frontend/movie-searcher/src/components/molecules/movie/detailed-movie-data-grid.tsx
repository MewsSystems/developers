import React from "react";
import styled from "styled-components";
import { DetailedMovie } from "../../../models/tmdbModels";
import { MovieData } from "../../atoms/movie/movie-data";

export const DetailedMovieDataGrid: React.FC<{
  detailedMovie: DetailedMovie;
}> = ({ detailedMovie }) => {
  return (
    <GridContainer>
      <MovieData
        title="Genres"
        content={detailedMovie.genres.map((g) => g.name).join(", ")}
      />
      <MovieData title="Release Date" content={detailedMovie.release_date} />
      <MovieData
        title="Budget"
        content={`$${detailedMovie.budget.toLocaleString()}`}
      />
      <MovieData
        title="IMDb"
        content={detailedMovie.title}
        link={`https://www.imdb.com/title/${detailedMovie.imdb_id}`}
      />
      <MovieData
        title="Original Language"
        content={detailedMovie.original_language}
      />
      <MovieData
        title="Origin Country"
        content={detailedMovie.origin_country.join(", ")}
      />
      <MovieData
        title="Revenue"
        content={`$${detailedMovie.revenue.toLocaleString()}`}
      />
      <MovieData title="Status" content={detailedMovie.status} />
      <MovieData
        title="Spoken Languages"
        content={detailedMovie.spoken_languages
          .map((language) => language.name)
          .join(", ")}
      />
    </GridContainer>
  );
};

const GridContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  grid-template-rows: repeat(4, auto);
  gap: 1rem;
`;
