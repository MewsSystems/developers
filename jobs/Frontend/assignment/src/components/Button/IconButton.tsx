import { ButtonHTMLAttributes, PropsWithChildren } from "react";
import styled from "styled-components";

type ButtonColor = "primary" | "secondary" | "tertiary" | "transparent";
type ButtonSize = "small" | "medium" | "large";

export interface IconButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  size?: ButtonSize;
  color?: ButtonColor;
}

const padding: { [key in ButtonSize]: string } = {
  small: "4px",
  medium: "8px",
  large: "16px",
};

const borderRadius: { [key in ButtonSize]: string } = {
  small: "8px",
  medium: "10px",
  large: "16px",
};

const StyledButton = styled.button<IconButtonProps>`
  display: grid;
  place-items: center;

  color: ${({ theme }) => theme.colors.surface.on};
  border: 1px solid
    ${({ theme, color = "transparent" }) =>
      color === "transparent" ? theme.colors.outline.variant : theme.colors[color].onFixedVariant};
  background-color: ${({ theme, color = "transparent" }) =>
    color === "transparent" ? "transparent" : theme.colors[color].container};

  border-radius: ${({ size = "medium" }) => borderRadius[size]};
  padding: ${({ size = "medium" }) => padding[size]};

  &:disabled {
    opacity: 0.5;
  }
`;

export function IconButton({ ...props }: PropsWithChildren<IconButtonProps>) {
  return <StyledButton {...props} />;
}
