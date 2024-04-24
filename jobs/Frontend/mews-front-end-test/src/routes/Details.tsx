import React, { FC } from 'react';
import { Header } from '../components/Header/Header';
import { StyledAppContainer } from './App.styled';

const Details: FC = () => {
  return (
    <StyledAppContainer>
      <Header />
      <div>Just a load of stuff about your film</div>
    </StyledAppContainer>
  );
};

export { Details };
