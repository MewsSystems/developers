"use client";

import { Colors, Sizes, Typography } from "@/types/styled";
import styled from "styled-components";
import { Base } from ".";
import { generateTextSize } from "@/util/styles";

interface Props {
  readonly $c?: keyof Colors;
  readonly $size?: keyof Sizes;
  readonly $fs?: keyof Sizes | number;
  readonly $lh?: keyof Sizes | number;
  readonly $ff?: keyof Typography["fontFamily"];
  readonly $fw?: number;
  readonly $ta?: "left" | "center" | "right";
}

export const Text = styled(Base).attrs({ as: "p" })<Props>`
  color: ${({ $c, theme }) => theme.colors[$c ?? "textPrimary"]};
  font-size: ${({ $size, $fs, theme }) => generateTextSize($size, $fs, theme)};
  line-height: ${({ $size, $lh, theme }) =>
    generateTextSize($size, $lh, theme)};
  font-family: ${({ $ff, theme }) =>
    theme.typography.fontFamily[$ff ?? "primary"]};
  font-weight: ${({ $fw }) => $fw ?? 400};
  text-align: ${({ $ta }) => $ta ?? "left"};
`;
