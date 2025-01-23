import React from "react";
import styled from "styled-components";

const Title = styled.h1`
  text-align: center;
  padding: 16px;
  background-color: ${(props) => props.theme.colors.background};
  color: white;
  margin: 0;
`;

const Header: React.FC = () => {
  return (
    <header>
      <Title>Movie search</Title>
    </header>
  );
};

export default Header;
