import React from 'react';
import styled from 'styled-components';

const Error404Container = styled.div`
  padding: 2rem;
`;

const Error404: React.FC = () => {
  return (
    <Error404Container>
      <h1>404 - Not Found</h1>
      <p>We couldn't find the movie you were looking for</p>
    </Error404Container>
  );
};

export default Error404;
