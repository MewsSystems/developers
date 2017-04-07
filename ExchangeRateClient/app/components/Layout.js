'use strict';

import React from 'react';
import { Link } from 'react-router';
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, NavbarBrand, NavbarHeader, NavbarCollapse} from 'react-bootstrap/lib';

export default class Layout extends React.Component {

  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="app-container">
        <div className="app-content">{this.props.children}</div>
      </div>
    );
  }
}
