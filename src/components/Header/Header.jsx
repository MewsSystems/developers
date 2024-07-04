import styled, { keyframes } from "styled-components";
import logo from "./logo.svg";
import { Form } from "../Form/Form";

const AppHeader = styled.header`
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: start;
  font-size: calc(10px + 2vmin);
`;

const rotate = keyframes`
from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

const RotatingLogo = styled.img`
  margin-top: calc(2 * (10px + 2vmin));
  height: 20vmin;
  pointer-events: none;

  @media (prefers-reduced-motion: no-preference) {
    animation: ${rotate} infinite 20s linear;
  }
`;

export const Header = () => {
  return (
    <AppHeader>
      <RotatingLogo src={logo} alt="logo" />
      <Form />
    </AppHeader>
  );
};
