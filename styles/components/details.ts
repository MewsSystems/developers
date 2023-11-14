"use client";

import { rem } from "@/util/styles";
import styled from "styled-components";

export const DetailsWrapper = styled.div<{ readonly $bgImage: string }>`
  position: relative;
  height: 100vh;

  background: url(${({ $bgImage }) => $bgImage});
  background-size: cover;
  background-repeat: no-repeat;
  background-position: top;

  display: flex;
  justify-content: center;
  align-items: flex-end;

  &::after {
    content: "";
    position: absolute;
    z-index: 1;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    background: linear-gradient(to bottom, transparent, black);
  }

  @media (max-width: ${({ theme }) => theme.breakpoint.md}) {
    height: auto;
    padding-top: 5rem;
  }
`;

export const DetailsContainer = styled.div`
  width: 90%;
  position: relative;
  z-index: 2;
  margin-top: ${rem(64)};
  padding-bottom: ${rem(125)};

  display: flex;
  justify-content: space-around;
  align-items: flex-start;
  gap: 4rem;
  flex-wrap: wrap;

  & > * {
    flex: 1;
  }
`;
