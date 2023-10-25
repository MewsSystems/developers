import React from 'react';

import { Nav, NavList, NavItem, StyledNavLink } from './styles';

export const NavBar = () => {
  return (
    <Nav>
      <NavList>
        <NavItem>
          <StyledNavLink to="/">Home</StyledNavLink>
        </NavItem>

        <NavItem>
          <StyledNavLink to="/movies">Movies</StyledNavLink>{' '}
        </NavItem>
      </NavList>
    </Nav>
  );
};
