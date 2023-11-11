import { AnchorHTMLAttributes, ButtonHTMLAttributes, PropsWithChildren } from "react";
import styled, { css } from "styled-components";

export interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {}
export interface LinkButtonProps extends AnchorHTMLAttributes<HTMLAnchorElement> {}

const styles = css`
  border: none;
  padding: 10px 24px;
  border-radius: 100px;
  text-decoration: none;

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

const StyledButton = styled.button`
  ${styles}
`;

const StyledLink = styled.a`
  ${styles}
`;

export function Button({ ...props }: PropsWithChildren<ButtonProps>) {
  return <StyledButton {...props} />;
}

export function LinkButton({ ...props }: PropsWithChildren<LinkButtonProps>) {
  return <StyledLink {...props} />;
}
