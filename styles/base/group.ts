"use client";

import { Sizes } from "@/types/styled";
import { generateSpacing } from "@/util/styles";
import { CSSProperties } from "react";
import styled from "styled-components";
import { Box } from "./box";

interface Props {
  readonly $align?: CSSProperties["alignItems"];
  readonly $justify?: CSSProperties["justifyContent"];
  readonly $gap?: keyof Sizes | number;
}

export const Group = styled(Box)<Props>`
  display: flex;
  align-items: ${({ $align }) => $align ?? "center"};
  justify-content: ${({ $justify }) => $justify ?? "flex-start"};
  gap: ${({ $gap, theme }) => generateSpacing($gap, theme)};
`;
