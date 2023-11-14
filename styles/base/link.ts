"use client";

import styled from "styled-components";
import { Text } from "./text";

interface Props {
  readonly $variant: "primary" | "secondary";
}

export const Link = styled(Text)<Props>`
  transition: color 0.5s ease, text-shadow 0.5s ease;

  &:hover {
    color: ${({ $variant, theme }) => theme.colors[$variant]};
    text-shadow: 2px 2px 7px ${({ $variant, theme }) => theme.colors[$variant]};
  }
`;
