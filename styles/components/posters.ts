"use client";

import styled from "styled-components";
import { Stack } from "../base/stack";

interface Props {
  readonly $bgImage: string;
}

export const PosterItem = styled.div<Props>`
  width: 15rem;
  height: 8.5rem;

  background: url(${({ $bgImage }) => $bgImage});
  background-position: center;
  background-repeat: no-repeat;
  background-size: 120%;

  opacity: 0.9;
  border-radius: 0.75rem;

  transition: background-size 0.5s ease-in-out, opacity 0.5s ease-in-out;

  &:hover {
    background-size: 135%;
    opacity: 1;
  }
`;

export const PostersWrapper = styled(Stack)`
  position: relative;

  .swiper {
    width: 100%;
    margin: 0;
    position: absolute;
  }
`;
