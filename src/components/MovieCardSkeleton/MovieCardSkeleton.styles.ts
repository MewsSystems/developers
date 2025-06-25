import styled, { keyframes } from "styled-components"

const shimmer = keyframes`
  0% {
    background-position: -468px 0;
  }
  100% {
    background-position: 468px 0;
  }
`

export const SkeletonBase = styled.div`
  background: linear-gradient(
    to right,
    ${({ theme }) => theme.colors.surface} 0%,
    ${({ theme }) => theme.colors.surfaceHover} 20%,
    ${({ theme }) => theme.colors.surface} 40%,
    ${({ theme }) => theme.colors.surface} 100%
  );
  background-size: 800px 104px;
  animation: ${shimmer} 1.5s linear infinite;
  border-radius: ${({ theme }) => theme.borderRadius.base};
`

export const SkeletonCard = styled.div`
  background-color: ${({ theme }) => theme.colors.surface};
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  overflow: hidden;
  display: flex;
  flex-direction: row;
  height: 180px;

  @media (min-width: ${({ theme }) => theme.breakpoints.md}) {
    flex-direction: column;
    height: auto;
  }
`

export const SkeletonPoster = styled(SkeletonBase)`
  flex-shrink: 0;
  width: 120px;
  height: 100%;

  @media (min-width: ${({ theme }) => theme.breakpoints.md}) {
    width: 100%;
    height: auto;
    aspect-ratio: 2/3;
  }
`

export const SkeletonContent = styled.div`
  padding: ${({ theme }) => theme.spacing.md};
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing.xs};
  flex: 1;
  min-width: 0;

  @media (min-width: ${({ theme }) => theme.breakpoints.md}) {
    gap: ${({ theme }) => theme.spacing.sm};
  }
`

export const SkeletonTitle = styled(SkeletonBase)`
  height: 20px;
  width: 80%;

  @media (min-width: ${({ theme }) => theme.breakpoints.md}) {
    height: 24px;
    margin-bottom: ${({ theme }) => theme.spacing.sm};
  }
`

export const SkeletonOverview = styled(SkeletonBase)`
  height: 14px;
  width: 100%;

  @media (min-width: ${({ theme }) => theme.breakpoints.md}) {
    height: 16px;
    margin-bottom: ${({ theme }) => theme.spacing.xs};
  }
`

export const SkeletonOverviewShort = styled(SkeletonBase)`
  height: 14px;
  width: 70%;
  flex: 1;

  @media (min-width: ${({ theme }) => theme.breakpoints.md}) {
    height: 16px;
    margin-bottom: ${({ theme }) => theme.spacing.sm};
    flex: none;
  }
`

export const SkeletonInfo = styled(SkeletonBase)`
  height: 12px;
  width: 60%;
  margin-top: auto;

  @media (min-width: ${({ theme }) => theme.breakpoints.md}) {
    height: 14px;
    margin-top: 0;
  }
`
