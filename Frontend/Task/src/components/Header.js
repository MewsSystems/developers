import React from 'react';
import { Link } from 'react-router-dom';

export const Header = () => (
  <header className="header">
    <h1 className="header__title">Exchange Rate Client</h1>
    <ul className="nav">
      <li className="nav__item">
        <Link
          className="nav__link"
          to="/">
          Home
        </Link>
      </li>
      <li className="nav__item">
        <Link
          className="nav__link"
          to="/author">
          Author
        </Link>
      </li>
    </ul>

  </header>
);



export default Header;
