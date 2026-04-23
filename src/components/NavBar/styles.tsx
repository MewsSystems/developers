import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { theme } from '../../styles/theme';

export const Nav = styled.nav`
  color: ${theme.colors.white[300]};
  width: 100%;
  border-bottom: 1px solid ${theme.colors.black[300]};
  background-color: ${theme.colors.yellow[300]};
`;

export const NavList = styled.ul`
  list-style: none;
  display: flex;
  padding: ${theme.spacing.md}px;
  padding: 0;
  margin: 0;
`;

export const NavItem = styled.li`
  margin: 0;
  padding: ${theme.spacing.md}px;

  cursor: pointer;
  display: flex;
  &:hover {
    background-color: ${theme.colors.yellow[400]};
  }
`;

export const StyledNavLink = styled(Link)`
  text-decoration: none;
  color: ${theme.colors.white[300]};
`;
