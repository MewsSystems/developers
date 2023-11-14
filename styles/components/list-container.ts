"use client";

import styled from "styled-components";
import { Stack } from "../base/stack";

export const ListContainer = styled(Stack)`
  max-width: 1280px;
  margin: 0 auto;
  margin-top: 4rem;
  padding: 1rem;

  .swiper-slide {
    width: 18%;

    @media (max-width: ${({ theme }) => theme.breakpoint.md}) {
      width: 25%;
    }
    @media (max-width: ${({ theme }) => theme.breakpoint.sm}) {
      width: 35%;
    }
    @media (max-width: ${({ theme }) => theme.breakpoint.xs}) {
      width: 45%;
    }
  }
`;
