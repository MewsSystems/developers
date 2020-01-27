import React from "react";
import { Link } from "react-router-dom";

//Styles
import "./nav.styles.scss";

const Nav = () => {
  return (
    <nav className="nav">
      <ul className="nav__list">
        <li>
          <Link to="/">Home</Link>
        </li>
        <li>
          <Link to="/aboutus">About us</Link>
        </li>
        <li>
          <Link to="/contact">Contact</Link>
        </li>
      </ul>
    </nav>
  );
};

export default Nav;
