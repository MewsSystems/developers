"use client";

import styled from "styled-components";
import { Text } from "../base/text";

export const InfoModal = styled.div`
  position: fixed;
  z-index: 999;
  left: 0;
  bottom: 0;
  width: 100%;
  height: 20rem;

  display: flex;
  align-items: stretch;
  justify-content: space-between;

  background: linear-gradient(
    90deg,
    rgba(42, 42, 42, 1) 0%,
    rgba(24, 24, 24, 1) 30%,
    rgba(0, 0, 0, 1) 100%
  );
`;

export const InfoModalContentImageBlock = styled.div`
  position: relative;
  height: 100%;
  width: 220px;
`;

export const InfoModalContentInfo = styled.div`
  flex: 1;
  padding: ${({ theme }) => theme.spacing.md};

  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: space-evenly;
`;

export const InfoModalContentInfoText = styled(Text)`
  display: -webkit-box;
  -webkit-line-clamp: 5;
  -webkit-box-orient: vertical;
  overflow: hidden;
`;

export const InfoModalImage = styled.div<{ readonly $bgImage: string }>`
  position: relative;
  min-width: 35%;

  background-image: url(${({ $bgImage }) => $bgImage});
  background-size: cover;
  background-repeat: no-repeat;
  background-position: center;
  opacity: 0.5;
`;

export const InfoModalCloseBlock = styled.div`
  position: absolute;
  right: 10px;
  top: 10px;
  height: 35px;
  width: 35px;
  border-radius: 100%;
  background-color: ${({ theme }) => theme.colors.background};
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
`;
