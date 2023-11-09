import { ButtonHTMLAttributes, PropsWithChildren } from "react";
import styled from "styled-components";

export interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {}

const StyledButton = styled.button`
  border: none;
  padding: 10px 24px;
  border-radius: 100px;

  font-size: ${props => props.theme.fonts.labelLarge.fontSize};
  color: ${props => props.theme.colors.primary.on};

  background-color: ${props => props.theme.colors.primary.main};

  &:hover {
    opacity: 0.8;
  }

  &:focus {
    outline: 2px solid ${props => props.theme.colors.primary.container};
    opacity: 0.9;
  }
`;

export function Button({ children, ...props }: PropsWithChildren<ButtonProps>) {
  return <StyledButton {...props}>{children}</StyledButton>;
}
