import { NavLink } from 'react-router-dom';
import styled from 'styled-components';

const Nav = styled.nav`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  padding: 15px 20px;
  background-color: #276278;

  p,
  a {
    font-size: 22px;
    color: #d29f3a;
  }

  p {
    margin: 0;
  }

  span {
    font-size: 20px;
  }
`;

const NavBar = () => {
  return (
    <Nav>
      <p>MOVIE</p>
      <NavLink to={'/'}>
        <span>&#128270;</span>Search
      </NavLink>
    </Nav>
  );
};

export default NavBar;
