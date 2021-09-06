import { NavLink } from 'react-router-dom';
import styled from 'styled-components';

const Nav = styled.nav`
  padding: 15px 10%;
  text-align: right;
  background-color: #bd7898;

  a {
    font-size: 22px;
    color: #1c0f27;
  }

  span {
    font-size: 20px;
  }

  @media (min-width: 768px) {
    padding: 20px 10%;
  }
`;

const NavBar = () => {
  return (
    <Nav>
      <NavLink to="/">
        <span>&#128270;</span>Search Movie
      </NavLink>
    </Nav>
  );
};

export default NavBar;
