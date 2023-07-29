import { FC } from "react";
import Styled, { keyframes } from "styled-components";
import spinner from "../../assets/512px-Spinner_font_awesome.png";

// Create the keyframes
const rotate = keyframes`
  from {
    transform: rotate(0deg);
  }

  to {
    transform: rotate(360deg);
  }
`;

const SpinnerStyled = Styled.div`
  display: block;
  animation: ${rotate} 2s linear infinite;
  padding: 2rem 1rem;
  font-size: 1.2rem;
`;

const Spinner: FC<{}> = () => {
  return (
    <SpinnerStyled>
      <img width={32} src={spinner} alt='spinner'></img>
    </SpinnerStyled>
  );
};

export default Spinner;
