"use client";

import styled from "styled-components";

export const Text = styled.div`
  background-color: ${({ theme }) => theme.colors.primary};
  padding: ${({ theme }) => theme.spacing.xl};
`;
