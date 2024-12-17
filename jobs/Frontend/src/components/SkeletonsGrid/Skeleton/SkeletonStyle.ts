import styled, { keyframes } from "styled-components";

const shimmer = keyframes`
  0% {
    background-position: -200px 0;
  }
  100% {
    background-position: calc(200px + 100%) 0;
  }
`;

export const SkeletonContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing(1)};
  background: ${({ theme }) => theme.colors.cardBackground};
  border-radius: 8px;
  padding: ${({ theme }) => theme.spacing(2)};
  overflow: hidden;
`;

export const SkeletonPoster = styled.div`
  width: 100%;
  height: 250px;
  border-radius: 4px;
  background: #444;
  background-image: linear-gradient(90deg, #444 0px, #555 40px, #444 80px);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;
`;

export const SkeletonTitle = styled.div`
  width: 70%;
  height: 12px;
  border-radius: 4px;
  background: #444;
  background-image: linear-gradient(90deg, #444 0px, #555 40px, #444 80px);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;
`;

export const SkeletonSubInfo = styled.div`
  width: 40%;
  height: 12px;
  border-radius: 4px;
  background: #444;
  background-image: linear-gradient(90deg, #444 0px, #555 40px, #444 80px);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;
`;
