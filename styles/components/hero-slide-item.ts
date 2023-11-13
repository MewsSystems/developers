"use client";

import styled from "styled-components";
import { Box } from "../base/box";
import Image from "next/image";

interface Props {
  readonly $isActive: boolean;
  readonly $bgImage: string;
}

export const HeroSlideItem = styled(Box)<Props>`
  width: 100%;
  height: 100%;
  padding-top: 4rem;

  background-image: url(${({ $bgImage }) => $bgImage});
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;

  &::before {
    content: "";
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: linear-gradient(rgba(0, 0, 0, 0.35), black);
  }

  &::after {
    content: "";
    position: absolute;
    left: 0;
    bottom: 0;
    width: 100%;
    height: 75px;
    background: linear-gradient(to top, $color-dark-blue, $color-black);
  }

  & > * {
    position: relative;
    z-index: 1;
  }
`;

export const HeroSlideItemImage = styled(Image)`
  border-radius: 1rem;
  box-shadow: rgba(100, 100, 111, 0.2) 0px 7px 29px 0px;
`;
