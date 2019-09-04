import React from 'react';
import { Link } from 'react-router-dom';

export const Header = () => (
  <header>
    <ul>
      <li>
        <Link to="/">
          Exchange Rate Client
        </Link>
      </li>
      <li>
        <Link to="/author">
          Author
        </Link>
      </li>
    </ul>

  </header>
);



export default Header;
