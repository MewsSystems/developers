// @flow strict
import * as React from "react";
import styled from "styled-components";
import { FaCaretUp, FaCaretDown, FaSnowflake } from "react-icons/fa";
import theme from "../theme";
import type { Theme } from "../theme/types";

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
  color: ${({ type, theme }: { type: Type, theme: Theme }) =>
    ({
      increased: theme.colors.success,
      decreased: theme.colors.alert,
      default: theme.colors.white,
    }[type])};
`;

Text.defaultProps = {
  theme,
};

const DownIcon = styled(FaCaretDown)`
  color: ${({ theme }: { theme: Theme }) => theme.colors.alert};
`;

DownIcon.defaultProps = {
  theme,
};

const UpIcon = styled(FaCaretUp)`
  color: ${({ theme }: { theme: Theme }) => theme.colors.success};
`;

UpIcon.defaultProps = {
  theme,
};

const Snowflake = styled(FaSnowflake)`
  color: ${({ theme }: { theme: Theme }) => theme.colors.color2};
`;

const getRateType = (current, before): Type => {
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
