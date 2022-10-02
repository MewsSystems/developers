import React from 'react';
import styled from 'styled-components';
import { Container } from '../../styles/Container.styled';
import SearchInput from './SearchInput';

const Header = styled.header`
  background: #212121;
  margin-bottom: 60px;
  padding: 120px 0 60px;
  
  h1 {
    color: #fff;
    text-align: center;
  }

  @media (max-width: 767px) {
    padding-top: 60px;
  }
`;

export default function HomeHeader() {
  return (
    <Header>
      <Container>
        <h1>Movie Database</h1>
        <SearchInput />
      </Container>
    </Header>
  );
}
