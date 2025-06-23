import styled, { keyframes } from "styled-components";
import logo from "./logo.svg";


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

export const Header : React.FC= ():JSX.Element => {
  return (
    <header>
      <RotatingLogo src={logo} alt="logo" />
    </header>
  );
};
