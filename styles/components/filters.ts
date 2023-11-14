"use client";

import styled from "styled-components";

export const Input = styled.input`
  padding: ${({ theme }) => theme.spacing.sm};
  font-family: ${({ theme }) => theme.typography.fontFamily.primary};
  color: ${({ theme }) => theme.colors.white};
  background-color: transparent;
  border: ${({ theme }) => `1px solid ${theme.colors.white}`};
`;

export const Select = styled.select`
  padding: ${({ theme }) => theme.spacing.xs};
  font-family: ${({ theme }) => theme.typography.fontFamily.primary};
  color: ${({ theme }) => theme.colors.white};
  background-color: transparent;
`;
