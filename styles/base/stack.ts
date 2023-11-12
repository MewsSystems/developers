"use client";

import styled from "styled-components";
import { Sizes } from "@/types/styled";
import { generateSpacing } from "@/util/styles";
import { CSSProperties } from "react";
import { Box } from "./box";

interface Props {
  readonly $align?: CSSProperties["alignItems"];
  readonly $justify?: CSSProperties["justifyContent"];
  readonly $gap?: keyof Sizes | number;
}

export const Stack = styled(Box)<Props>`
  display: flex;
  flex-direction: column;
  align-items: ${({ $align }) => $align ?? "stretch"};
  justify-content: ${({ $justify }) => $justify ?? "flex-start"};
  gap: ${({ $gap, theme }) => generateSpacing($gap, theme)};
`;
