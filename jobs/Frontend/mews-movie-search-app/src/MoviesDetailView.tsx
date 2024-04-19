/* Global imports */
import * as React from "react";
import styled from "styled-components";
import { useParams } from "wouter";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const MoviesDetailView = () => {
  const params = useParams<{ id: string }>();

  return (
    <MovieDetailLayout>
      <h1>Movie Details for id: {params.id} </h1>
      <p>Movie Details</p>
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
