"use client";

import styled from "styled-components";
import { Box } from "../base/box";
import { rem } from "@/util/styles";
import { Title } from "../base/title";

interface Props {
  readonly $bgImage: string;
}

export const ListItem = styled(Box)<Props>`
  position: relative;
  padding-top: 150%;
  margin-top: 1rem;

  background-image: url(${({ $bgImage }) => $bgImage});
  background-position: center;
  background-size: cover;
  background-repeat: no-repeat;
  transition: all 0.35s ease;

  border-radius: ${rem(24)};
  cursor: pointer;

  &::before {
    content: "";
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    border-radius: ${rem(24)};
    transition: ${({ theme }) => theme.colors.background} 0.35s ease-in-out;
  }

  &:hover {
    transform: translateY(-${rem(16)});
  }

  &:hover::before {
    background-color: ${({ theme }) => theme.colors.black};
    opacity: 0.9;
  }
`;

export const ListItemTitle = styled(Title)`
  position: absolute;
  width: 100%;
  height: 100%;
  left: 50%;
  top: 50%;
  opacity: 0;
  transform: translate(-50%, -50%);
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  transition: opacity 0.5s ease;

  &:hover {
    opacity: 1;
  }
`;
