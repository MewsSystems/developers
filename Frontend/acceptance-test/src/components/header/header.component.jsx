import React, { useState } from "react";
import { Link } from "react-router-dom";

//Components
import Search from "../search-bar/search-bar.component";
import Nav from "../nav/nav.component";

//Styles
import "./header.styles.scss";

export const Header = () => {
  const [placeholder] = useState("Search...");

  return (
    <header className="header">
      <Link to="/" className="logo">
        RS
      </Link>
      <Search placeholder={placeholder} />
      <Nav />
    </header>
  );
};

export default Header;
