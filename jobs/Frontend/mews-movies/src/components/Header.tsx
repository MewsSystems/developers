import React from "react";
import { useNavigate, useLocation } from "react-router-dom";
import styled from "styled-components";
import { handleBackNavigation } from "../utils/navigationUtils";

const HeaderContainer = styled.header`
  display: flex;
  height: 60px;
  padding: 0 2rem;
  align-items: center;
  background-color: #4b83f1;
  color: white;
`;

const BackButton = styled.button`
  margin-right: 1rem;
  padding: 0.5rem;
  background: none;
  border: none;
  color: white;
  font-size: 1.5rem;
  cursor: pointer;
  transition: all 0.5s ease;

  &:hover {
    background-color: #dee9ff;
    color: #4b83f1;
    border-radius: 10px;
  }
`;

const SiteName = styled.a`
  font-size: 1.5rem;
  text-decoration: none;
  color: white;
`;

const Header: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();

  return (
    <HeaderContainer>
      {location.pathname !== "/" && (
        <BackButton onClick={() => handleBackNavigation(navigate)}>
          &larr; Back
        </BackButton>
      )}
      <SiteName href="/">Movie Search</SiteName>
    </HeaderContainer>
  );
};

export default Header;
