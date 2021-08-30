import { NavLink } from 'react-router-dom';

const Nav = () => {
  return (
    <nav>
      <p>MOVIE</p>
      <NavLink to={'/'}>
        <span>&#128270;</span>Search
      </NavLink>
    </nav>
  );
};

export default Nav;
