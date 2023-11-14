"use client";

import styled from "styled-components";
import { Group } from "../base/group";
import { Title } from "../base/title";
import bgHeroCover from "@/public/images/home-hero-bg.jpg";
import { rem } from "@/util/styles";

export const LandingHeroSection = styled(Group)`
  position: relative;
  width: 100%;
  height: 100vh;
  background-image: url(${bgHeroCover.src});
  background-repeat: no-repeat;
  background-size: cover;
  background-position: bottom;

  &::before {
    content: "";
    position: absolute;
    width: 100%;
    height: 100%;
    left: 0;
    top: 0;
    background-color: ${({ theme }) => theme.colors.black};
    opacity: 0.85;
  }

  & > * {
    z-index: 1;
  }

  @media (max-width: ${({ theme }) => theme.breakpoint.sm}) {
    flex-direction: column;
    gap: ${({ theme }) => theme.spacing.xl};
    margin-top: 5rem;
  }
`;

export const HeroTitle = styled(Title)`
  background: ${({ theme }) =>
    `linear-gradient(89.97deg, ${theme.colors.secondary} 1%, ${theme.colors.primary} 77.67%)`};
  background-clip: text;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;

  @media (max-width: ${({ theme }) => theme.breakpoint.sm}) {
    font-size: ${rem(96)};
    line-height: ${rem(108)};
  }
`;
