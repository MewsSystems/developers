import { PropsWithChildren } from "react";
import { TypographyVariant } from "@/theme/types";
import styled from "styled-components";

export interface TypographyProps {
  variant: TypographyVariant;
  element?: "p" | "span" | "h1" | "h2" | "h3" | "h4" | "h5" | "h6" | "div";
  bold?: boolean;
}

const StyledTypography = styled.p<TypographyProps>`
  font-size: ${({ theme, variant }) => theme.fonts[variant].fontSize};
  font-weight: ${({ theme, variant, bold }) => (bold ? "bold" : theme.fonts[variant].fontWeight)};
`;

export function Typography({
  element,
  variant = "bodyMedium",
  ...props
}: PropsWithChildren<TypographyProps>) {
  return <StyledTypography as={element} variant={variant} {...props} />;
}
