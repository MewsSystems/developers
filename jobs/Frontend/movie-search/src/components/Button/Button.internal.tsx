import styled from "styled-components";

interface StyledButtonProps {
  $isCircle?: boolean;
}

export const StyledButton = styled.button<StyledButtonProps>`
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: inherit;
  text-decoration: none;
  color: #333;
  font-size: 0.8rem;
  font-weight: 400;
  background-color: #eee;
  padding: 0.8rem;
  border-radius: ${(props) => (props.$isCircle ? "50px" : 0)};

  transition: all 0.3s ease-out;

  cursor: pointer;

  &:hover {
    background-color: #ddd;
  }
`;
