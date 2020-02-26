import React from 'react';
import { Link } from 'react-router-dom'

const NavBar = () => {
  return (
    <nav>
      <div className="nav-wrapper blue">
        <span href="#" className="brand-logo" style={{ paddingLeft: '10px'}}>
          <i className="material-icons">movie</i> The Movie DB
        </span>
        <ul id="nav-mobile" className="right hide-on-med-and-down">
          <li>
            <Link to='/'>Home</Link>
          </li>
          <li>
            <Link to='/about'>About</Link>
          </li>
        </ul>
      </div>
    </nav>
  )
};

export default NavBar;
