// @flow strict
import * as React from "react";
import styled from "styled-components";
import { FaCaretUp, FaCaretDown, FaSnowflake } from "react-icons/fa";
import defaultTheme from "../theme";
import type { Theme as ThemeType } from "../theme/types";

type Type = "decreased" | "increased" | "default";

const Container = styled.div`
  flex: 3;
  display: inline-flex;
  position: relative;
`;

const IconContainer = styled.div`
  position: absolute;
`;

const Text = styled.span`
  margin-left: 1em;
  color: ${({ type, theme }: { type: Type, theme: ThemeType }) =>
    ({
      increased: theme.colors.success,
      decreased: theme.colors.alert,
      default: theme.colors.white,
    }[type])};
`;

Text.defaultProps = {
  theme: defaultTheme,
};

const DownIcon = styled(FaCaretDown)`
  color: ${({ theme }: { theme: ThemeType }) => theme.colors.alert};
`;

DownIcon.defaultProps = {
  theme: defaultTheme,
};

const UpIcon = styled(FaCaretUp)`
  color: ${({ theme }: { theme: ThemeType }) => theme.colors.success};
`;

UpIcon.defaultProps = {
  theme: defaultTheme,
};

const Snowflake = styled(FaSnowflake)`
  color: ${({ theme }: { theme: ThemeType }) => theme.colors.color2};
`;

Snowflake.defaultProps = {
  theme: defaultTheme,
};

const getRateType = (current, before = 0): Type => {
  if (before > current) {
    return "decreased";
  }
  if (current > before) {
    return "increased";
  }
  return "default";
};

const Icon = ({ type }: { type: Type }) => {
  if (type === "decreased") return <DownIcon data-testid="icon" />;
  if (type === "increased") return <UpIcon data-testid="icon" />;
  return <Snowflake size={25} data-testid="icon" />;
};

type Props = {| +current: number, +before: number |};

export default ({ current, before }: Props) => (
  <Container>
    <IconContainer>
      <Icon type={getRateType(current, before)} />
    </IconContainer>
    <Text type={getRateType(current, before)}>{current}</Text>
  </Container>
);
