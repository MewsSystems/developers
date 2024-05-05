"use client";

import { tabletMediaQuery } from "@/breakpoints";
import styled from "styled-components";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  margin-top: 30px;
  padding: 0 20px;
  text-align: center;

  ${tabletMediaQuery} {
    align-items: center;
  }
`;

interface NoQueryOrResultsProps {
  moviesCount: number;
  query: string;
}

const NoQueryOrResults = ({ moviesCount, query }: NoQueryOrResultsProps) => {
  return (
    <Container>
      {moviesCount === 0 && query ? (
        <>No movies found. Please try another search</>
      ) : (
        <>
          <p>
            Search for a movie using the search bar above to see movie results
          </p>
          <p>Click on a movie to view more details about it</p>
          {/* This would be a good screen to put movie categories, popular movies, etc on it */}
        </>
      )}
    </Container>
  );
};

export default NoQueryOrResults;
