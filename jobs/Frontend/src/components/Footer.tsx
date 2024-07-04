import React from "react";
import styled from "styled-components";

const CenteredText = styled.p`
  text-align: center;
  margin-top: auto;
`;

const FooterContainer = styled.footer`
  padding-top: 16px;
  display: flex;
  flex-direction: column;
  background-color: ${(props) => props.theme.colors.background};
  color: ${(props) => props.theme.colors.secondary};
`;

const Footer: React.FC = () => {
  return (
    <FooterContainer>
      <CenteredText>
        &copy; 2024 Mews front-end test - made by François Cabrol with ❤️
      </CenteredText>
    </FooterContainer>
  );
};

export default Footer;
