// @flow strict
import styled from "styled-components";
import * as React from "react";
import { FaExclamationTriangle } from "react-icons/fa";

const Container = styled.div`
  display: flex;
  padding: 2rem 3rem;
  color: ${({ theme }) => theme.colors.alert};
  font-weight: bold;
  font-size: 2em;
`;
const Icon = styled(FaExclamationTriangle)`
  margin-right: 20px;
`;

export default ({ error }: { error: Error }) => (
  <Container>
    <Icon /> ERROR: {error.message}
  </Container>
);
