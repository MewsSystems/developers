import React from "react";
import { HeaderWrapper } from './Header.styles';
import { withRouter } from 'react-router-dom';

const Header = (props) => {
  return (
    <HeaderWrapper onClick={ () => props.history.push('/') }>
      { props.text }
    </HeaderWrapper>
  );
};

export default withRouter(Header);
