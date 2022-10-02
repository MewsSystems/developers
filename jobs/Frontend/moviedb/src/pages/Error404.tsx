import React from 'react';
import { Link } from 'react-router-dom';
import { Container } from '../styles/Container.styled';

function Error404() {
  return (
    <Container>
      <h1>Page not found</h1>
      <Link to="/">Go back to homepage</Link>
    </Container>
  );
}

export default Error404;
