import React from 'react';

import Container from '../../../../atoms/Container/Container';

import StyledHeader from './styles/StyledHeader';


const Header = () => (
  <StyledHeader>
    <Container>
      <div className="header--container">

        <span className="header--title">AppName</span>

      </div>
    </Container>
  </StyledHeader>
);


export default Header;
