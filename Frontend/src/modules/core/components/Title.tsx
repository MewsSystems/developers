import React from 'react';
import styled from 'styled-components';

const H1 = styled.h1`
  text-align: center;
  display: block;
  font-size: 50px;
`;

export default function Title() {
    return <H1>Movie Search Engine</H1>;
}
