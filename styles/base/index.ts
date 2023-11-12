"use client";

import styled from "styled-components";
import { Colors, Sizes } from "@/types/styled";
import { generateSpacing, rem } from "@/util/styles";

interface Props {
  readonly $w?: number;
  readonly $miw?: number;
  readonly $maw?: number;
  readonly $h?: number;
  readonly $mih?: number;
  readonly $mah?: number;
  readonly $m?: keyof Sizes | number;
  readonly $mx?: keyof Sizes | number;
  readonly $my?: keyof Sizes | number;
  readonly $p?: keyof Sizes | number;
  readonly $px?: keyof Sizes | number;
  readonly $py?: keyof Sizes | number;
  readonly $bg?: keyof Colors;
}

export const Base = styled.div<Props>`
  width: ${({ $w }) => ($w ? rem($w) : "unset")};
  min-width: ${({ $miw }) => ($miw ? rem($miw) : "unset")};
  max-width: ${({ $maw }) => ($maw ? rem($maw) : "unset")};
  height: ${({ $h }) => ($h ? rem($h) : "unset")};
  min-height: ${({ $mih }) => ($mih ? rem($mih) : "unset")};
  max-height: ${({ $mah }) => ($mah ? rem($mah) : "unset")};
  margin: ${({ $m, theme }) => generateSpacing($m, theme)};
  margin-inline: ${({ $mx, theme }) => generateSpacing($mx, theme)};
  margin-block: ${({ $my, theme }) => generateSpacing($my, theme)};
  padding: ${({ $p, theme }) => generateSpacing($p, theme)};
  padding-inline: ${({ $px, theme }) => generateSpacing($px, theme)};
  padding-block: ${({ $py, theme }) => generateSpacing($py, theme)};
  background-color: ${({ $bg, theme }) =>
    $bg ? theme.colors[$bg] : "inherit"};
`;
