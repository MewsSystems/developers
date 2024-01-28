import styled from "styled-components";

export const StyledCloseButton = styled.button`
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background-color: transparent;
  border: none;
  cursor: pointer;
  position: relative;
  transition: background-color 0.3s ease-in-out;
  margin: 0.5rem;

  &::before,
  &::after {
    content: "";
    position: absolute;
    top: 50%;
    left: 25%;
    width: 55%;
    height: 2px;
    background-color: white;
    transform: translate(-50%, -50%);
  }

  &::before {
    transform: rotate(45deg);
  }

  &::after {
    transform: rotate(-45deg);
  }

  &:hover {
    background-color: #333;
  }
`;
