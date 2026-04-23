import styled, { keyframes } from "styled-components";

export const AppContainer = styled.div`
  background-color: #141414;
  color: white;
  min-height: 100vh;
  padding: 20px;
`;

export const Header = styled.h1`
  color: #e50914;
  font-size: 2.5rem;
  text-align: center;
  margin-bottom: 2rem;
`;

export const fadeIn = keyframes`
  from {
    opacity: 0;
  }

  to {
    opacity: 1;
  }
`;

export const MovieName = styled.span`
  color: white;
  display: inline-block; // Ensure the animation applies correctly
  animation: ${fadeIn} 0.5s ease-in-out;
`;
