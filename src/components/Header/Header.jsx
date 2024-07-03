import styled, { keyframes } from "styled-components";
import logo from "./logo.svg";

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

const StyledFormLabel = styled.label`
  color: black;
  display: flex;
  align-items: center;
  justify-content: start;
  font-size: calc(10px + 2vmin);
  margin: calc(10px + 2vmin);
`;

const Input = styled.input`
  width: 60%;
  &:not(:focus) {
    box-shadow: 0 0 2px 2px #ff6a00;
  }
`;

export const Header = () => {
  return (
    <AppHeader>
      <RotatingLogo src={logo} alt="logo" />
      <form>
        <StyledFormLabel htmlFor="movie-input">
          What would you like to find?
        </StyledFormLabel>
        <Input
          id="movie-input"
          type="text"
          placeholder="Type your search here"
        />
      </form>
    </AppHeader>
  );
};
