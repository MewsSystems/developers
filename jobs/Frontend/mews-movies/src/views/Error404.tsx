import React from "react";
import { useNavigate } from "react-router-dom";
import styled from "styled-components";

const Error404Container = styled.div`
  padding: 4rem;
`;

const ErrorTitle = styled.h1`
  font-size: 3rem;
`;

const ButtonElement = styled.button`
  padding: 1rem;
  margin: 2rem;
  border-radius: 0.5rem;
  background-color: #4b83f1;
  border: none;
  color: white;
  font-size: 1rem;
  cursor: pointer;

  &:hover {
    background-color: #5ca0ff;
  }
`;

const Error404: React.FC = () => {
  const navigate = useNavigate();
  const handleBackToStart = () => {
    navigate("/");
  };

  return (
    <Error404Container>
      <ErrorTitle>404 - Not Found</ErrorTitle>
      <p>Something went wrong</p>
      <ButtonElement onClick={handleBackToStart}>Back to Start</ButtonElement>
    </Error404Container>
  );
};

export default Error404;
