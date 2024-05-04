"use client";

import styled from "styled-components";

const Container = styled.div`
  border: 1px solid ${(props) => props.theme.primary.border};
`;

const TestComponent = () => {
  return <Container>Test</Container>;
};

export default TestComponent;
