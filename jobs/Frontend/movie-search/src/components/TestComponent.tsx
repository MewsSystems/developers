"use client";
import React from "react";
import styled from "styled-components";

const Heading = styled.h1`
  color: ${({ theme }) => theme.primary.text};
  background: ${({ theme }) => theme.primary.background};
  font-size: 2rem;
`;

export default function TestComponent() {
  return <Heading>hello world</Heading>;
}
