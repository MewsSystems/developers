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
  readonly $mt?: keyof Sizes | number;
  readonly $mb?: keyof Sizes | number;
  readonly $ml?: keyof Sizes | number;
  readonly $mr?: keyof Sizes | number;
  readonly $p?: keyof Sizes | number;
  readonly $px?: keyof Sizes | number;
  readonly $py?: keyof Sizes | number;
  readonly $pt?: keyof Sizes | number;
  readonly $pb?: keyof Sizes | number;
  readonly $pl?: keyof Sizes | number;
  readonly $pr?: keyof Sizes | number;
  readonly $bg?: keyof Colors;
  readonly $fullWidth?: boolean;
  readonly $fullHeight?: boolean;
}

export const Base = styled.div<Props>`
  width: ${({ $w, $fullWidth }) =>
    $fullWidth ? "100%" : $w ? rem($w) : "unset"};
  min-width: ${({ $miw }) => ($miw ? rem($miw) : "unset")};
  max-width: ${({ $maw }) => ($maw ? rem($maw) : "unset")};
  height: ${({ $h, $fullHeight }) =>
    $fullHeight ? "100%" : $h ? rem($h) : "unset"};
  min-height: ${({ $mih }) => ($mih ? rem($mih) : "unset")};
  max-height: ${({ $mah }) => ($mah ? rem($mah) : "unset")};
  margin: ${({ $m, theme }) => generateSpacing($m, theme)};
  margin-inline: ${({ $mx, theme }) => generateSpacing($mx, theme)};
  margin-block: ${({ $my, theme }) => generateSpacing($my, theme)};
  margin-top: ${({ $mt, theme }) => generateSpacing($mt, theme)};
  margin-bottom: ${({ $mb, theme }) => generateSpacing($mb, theme)};
  margin-left: ${({ $ml, theme }) => generateSpacing($ml, theme)};
  margin-right: ${({ $mr, theme }) => generateSpacing($mr, theme)};
  padding: ${({ $p, theme }) => generateSpacing($p, theme)};
  padding-inline: ${({ $px, theme }) => generateSpacing($px, theme)};
  padding-block: ${({ $py, theme }) => generateSpacing($py, theme)};
  padding-top: ${({ $pt, theme }) => generateSpacing($pt, theme)};
  padding-bottom: ${({ $pb, theme }) => generateSpacing($pb, theme)};
  padding-left: ${({ $pl, theme }) => generateSpacing($pl, theme)};
  padding-right: ${({ $pr, theme }) => generateSpacing($pr, theme)};
  background-color: ${({ $bg, theme }) =>
    $bg ? theme.colors[$bg] : "inherit"};
`;
