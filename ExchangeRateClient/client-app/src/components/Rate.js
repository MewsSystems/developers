// @flow strict
import * as React from "react";
import styled from "styled-components";
import { FaCaretUp, FaCaretDown } from "react-icons/fa";

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
  color: ${({ type, theme }) =>
    ({
      increased: theme.colors.success,
      decreased: theme.colors.alert,
      default: theme.colors.white,
    }[type])};
`;

const DownIcon = styled(FaCaretDown)`
  color: ${({ theme }) => theme.colors.alert};
`;

const UpIcon = styled(FaCaretUp)`
  color: ${({ theme }) => theme.colors.success};
`;

const getRateType = (current, before) => {
  if (before > current) {
    return "decreased";
  }
  if (current > before) {
    return "increased";
  }
  return "default";
};

const Icon = ({ type }) => {
  if (type === "decreased") return <DownIcon />;
  if (type === "increased") return <UpIcon />;
  return null;
};

export default ({ current, before }: { current: number, before: number }) => (
  <Container>
    <IconContainer>
      <Icon type={getRateType(current, before)} />
    </IconContainer>
    <Text type={getRateType(current, before)}>{current}</Text>
  </Container>
);
