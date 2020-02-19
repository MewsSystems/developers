import React from "react";
import { HeaderWrapper } from './Header.styles'

const Header = (props) => {
  return (
    <HeaderWrapper>
      { props.text }
    </HeaderWrapper>
  );
};

export default Header;
