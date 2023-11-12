"use client";

import { Colors, Typography } from "@/types/styled";
import styled from "styled-components";
import { Base } from ".";
import { generateTitleVariant, rem } from "@/util/styles";

interface Props {
  readonly $variant?: keyof Typography["headings"]["sizes"];
  readonly $fs?: number;
  readonly $lh?: number;
  readonly $c?: keyof Colors;
  readonly $ff?: keyof Typography["fontFamily"];
  readonly $fw?: number;
  readonly $ta?: "left" | "center" | "right";
}

export const Title = styled(Base).attrs<Props>(({ $variant }) => ({
  as: $variant,
}))<Props>`
  color: ${({ $c, theme }) => theme.colors[$c ?? "textPrimary"]};
  font-size: ${({ $variant, $fs, theme }) =>
    generateTitleVariant($variant ?? "h1", $fs, theme, "fontSize")};
  line-height: ${({ $variant, $lh, theme }) =>
    generateTitleVariant($variant ?? "h1", $lh, theme, "lineHeight")};
  font-family: ${({ $ff, theme }) =>
    theme.typography.fontFamily[$ff ?? "primary"]};
  font-weight: ${({ $fw, theme }) =>
    $fw ?? theme.typography.headings.fontWeight};
  text-align: ${({ $ta }) => $ta ?? "left"};
`;
