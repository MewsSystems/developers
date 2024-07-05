import React, { FC } from 'react';
import { StyledHeader, StyledTitle } from './Header.styled';

const Header: FC = () => {
  return (
    <StyledHeader>
      <StyledTitle>Search for movies</StyledTitle>
    </StyledHeader>
  );
};

export { Header };
