import styled, { keyframes } from "styled-components";

const rotate = keyframes`
  from {
    transform: rotate(0deg);
  }

  to {
    transform: rotate(360deg);
  }
`;

export const SpinnerWrapper = styled.div`
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
`;

export const SpinnerBody = styled.div`
  border: 4px solid ${props => props.theme.backgroundColorHover};
  border-top: 4px solid ${props => props.theme.highlighted};
  border-radius: 50%;
  width: 35px;
  height: 35px;
  animation: ${rotate} 2s linear infinite;
`;
