import React from 'react';
import Toolbar from '@mui/material/Toolbar';
import { StyledAppBar, StyledHeaderImg } from './header.styled';
import logo from '../../assets/images/movie-search-logo.png';

const Header = () => {
  return (
    <StyledAppBar position="static">
      <Toolbar>
        <StyledHeaderImg src={logo} alt="Movie Search Logo" />
      </Toolbar>
    </StyledAppBar>
  );
};

export default Header;
