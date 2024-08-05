import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import styled from 'styled-components';

const HeaderContainer = styled.header`
  display: flex;
  align-items: center;
  padding: 1rem;
  background-color: #4b83f1;
  color: white;
`;

const BackButton = styled.button`
  margin-right: 1rem;
  background: none;
  border: none;
  color: white;
  font-size: 1.5rem;
  cursor: pointer;

  &:hover {
    text-decoration: underline;
  }
`;

const Logo = styled.div`
  font-size: 1.5rem;
`;

const Header: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();

  return (
    <HeaderContainer>
      {location.pathname !== '/' && (
        <BackButton onClick={() => navigate(-1)}>&larr; Back</BackButton>
      )}
      <Logo>Movie Search</Logo>
    </HeaderContainer>
  );
};

export default Header;
