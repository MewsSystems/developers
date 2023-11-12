import { Sizes, Typography } from "@/types/styled";
import { DefaultTheme } from "styled-components/dist/types";

export const rem = (pixels: number) => `${pixels / 16}rem`;

export const generateSpacing = (
  prop: keyof Sizes | number | undefined,
  theme: DefaultTheme
) =>
  prop
    ? typeof prop === "number"
      ? rem(prop)
      : theme.spacing[(prop as keyof Sizes) ?? "md"]
    : "auto";

export const generateTextSize = (
  size: keyof Sizes | undefined,
  value: keyof Sizes | number | undefined,
  theme: DefaultTheme
) =>
  typeof value === "number"
    ? rem(value)
    : theme.typography.fontSizes[value ?? size ?? "md"];

export const generateTitleVariant = (
  variant: keyof Typography["headings"]["sizes"],
  value: number | undefined,
  theme: DefaultTheme,
  attribute: keyof Typography["headings"]["sizes"]["h1"]
) =>
  typeof value === "number"
    ? rem(value)
    : theme.typography.headings.sizes[variant][attribute];
