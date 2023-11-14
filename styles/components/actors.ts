"use client";

import styled from "styled-components";

export const Actors = styled.div``;

export const ActorItem = styled.div<{ readonly $bgImage: string }>`
  width: 5rem;
  height: 5rem;

  background: url(${({ $bgImage }) => $bgImage});
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;

  border-radius: 100%;
  border: ${({ theme }) => `2px solid ${theme.colors.background}`};
`;
