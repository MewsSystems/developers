/* Global imports */
import * as React from "react";
import styled from "styled-components";
import { useParams } from "wouter";
import { useMovieDetail } from "./useMovieDetail";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const MoviesDetailView = () => {
  const params = useParams<{ id: string }>();
  const { movieDetail } = useMovieDetail(Number(params.id));

  return (
    <MovieDetailLayout>
      <h1>Movie Details for id: {params.id} </h1>
      <p>Movie Details</p>
      <Debug>{JSON.stringify(movieDetail)}</Debug>
    </MovieDetailLayout>
  );
};
const MovieDetailLayout = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 1rem;
`;
const Debug = styled.pre`
  background-color: #f4f4f4;
  padding: 1rem;
  margin-top: 1rem;
  white-space: pre-wrap;
`;
