import styled, { keyframes } from "styled-components";

const shimmer = keyframes`
  0% {
    background-position: -200px 0;
  }
  100% {
    background-position: calc(200px + 100%) 0;
  }
`;

export const SkeletonDetailsContainer = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
`;

export const SkeletonHero = styled.div`
  width: 100%;
  height: 60vh;
  background: #444;
  background-image: linear-gradient(90deg, #444 0px, #555 40px, #444 80px);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;

  @media (min-width: 768px) {
    height: 70vh;
  }
`;

export const SkeletonContent = styled.div`
  flex: 1;
  max-width: 800px;
  padding: ${({ theme }) => theme.spacing(4)} ${({ theme }) => theme.spacing(2)};
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing(3)};

  @media (min-width: 768px) {
    padding: ${({ theme }) => theme.spacing(6)}
      ${({ theme }) => theme.spacing(3)};
  }
`;

interface SkeletonLineProps {
  height?: string;
  width?: string;
}

export const SkeletonLine = styled.div<SkeletonLineProps>`
  height: ${(props) => (props.height ? props.height : "12px")};
  width: ${(props) => (props.width ? props.width : "100%")};
  background: #444;
  border-radius: 4px;
  background-image: linear-gradient(90deg, #444 0px, #555 40px, #444 80px);
  background-size: 200px 100%;
  animation: ${shimmer} 1.5s infinite linear;
`;
