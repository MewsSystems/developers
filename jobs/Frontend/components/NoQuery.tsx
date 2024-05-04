"use client";

import styled from "styled-components";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 30px;
`;

const NoQuery = () => {
  return (
    <Container>
      <p>Search for a movie using the search bar above to see movie results</p>
      <p>Click on a movie to view more details about it</p>
      {/* This would be a good screen to put movie categories, popular movies, etc on it */}
    </Container>
  );
};

export default NoQuery;
