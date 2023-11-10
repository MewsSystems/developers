import { PropsWithChildren } from "react";
import { TypographyVariant } from "@/theme/types";
import styled, { DefaultTheme } from "styled-components";

export type TypographyColor = "surface" | "primary" | "secondary";

export interface TypographyProps {
  variant?: TypographyVariant;
  element?: "p" | "span" | "h1" | "h2" | "h3" | "h4" | "h5" | "h6" | "div";
  color?: TypographyColor;
  bold?: boolean;
}

const getColorByVariant = (theme: DefaultTheme, color?: TypographyColor) => {
  if (color === "surface") return theme.colors.surface.main;
  if (color === "secondary") return theme.colors.surface.onVariant;

  return theme.colors.surface.on;
};

const StyledTypography = styled.p<TypographyProps>`
  font-size: ${({ theme, variant }) => theme.fonts[variant || "bodyMedium"].fontSize};
  font-weight: ${({ theme, variant, bold }) =>
    bold ? "500" : theme.fonts[variant || "bodyMedium"].fontWeight};
  color: ${({ theme, color }) => getColorByVariant(theme, color)};
`;

export function Typography({
  element,
  variant = "bodyMedium",
  color = "primary",
  bold,
  children,
}: PropsWithChildren<TypographyProps>) {
  return (
    <StyledTypography as={element} bold={bold} variant={variant} color={color}>
      {children}
    </StyledTypography>
  );
}
