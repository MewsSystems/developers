"use client";

import { rem } from "@/util/styles";
import styled from "styled-components";

export const Button = styled.button`
  padding: ${({ theme }) => `${theme.spacing.sm} ${theme.spacing.lg}`};
  font-family: ${({ theme }) => theme.typography.fontFamily.primary};
  font-weight: 600;
  text-transform: capitalize;
  border: ${({ theme }) => `1px solid ${theme.colors.black}`};
  border-radius: ${rem(12)};
  cursor: pointer;
`;

export const PrimaryButton = styled(Button)`
  background-color: ${({ theme }) => theme.colors.primary};
  color: ${({ theme }) => theme.colors.white};
`;

export const SecondaryButton = styled(Button)`
  background-color: ${({ theme }) => theme.colors.secondary};
  color: ${({ theme }) => theme.colors.white};
`;
