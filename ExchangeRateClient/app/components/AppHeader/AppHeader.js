import React from "react";
import styled from "styled-components";

const Header = styled.h1`
  font-weight: 500;
  line-height: normal;
  font-size: 38px;
  color: #383838;
  padding: 90px 0 0 90px;
`;

const AppHeader = () => <Header>Exchange Rates</Header>;

export default AppHeader;
