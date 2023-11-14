"use client";

import styled from "styled-components";
import { Box } from "../base/box";
import { rem } from "@/util/styles";

export const GridList = styled(Box)`
  max-width: 100%;
  margin: 0 auto;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(${rem(225)}, 1fr));
  gap: ${({ theme }) => theme.spacing.md};
`;
