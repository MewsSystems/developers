import styled, { keyframes } from "styled-components"

const shimmer = keyframes`
  0% {
    background-position: -468px 0;
  }
  100% {
    background-position: 468px 0;
  }
`

const SkeletonBase = styled.div`
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

const SkeletonCard = styled.div`
  background-color: ${({ theme }) => theme.colors.surface};
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  overflow: hidden;
`

const SkeletonPoster = styled(SkeletonBase)`
  aspect-ratio: 2/3;
  width: 100%;
`

const SkeletonContent = styled.div`
  padding: ${({ theme }) => theme.spacing.md};
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing.sm};
`

const SkeletonTitle = styled(SkeletonBase)`
  height: 24px;
  width: 80%;
  margin-bottom: ${({ theme }) => theme.spacing.sm};
`

const SkeletonOverview = styled(SkeletonBase)`
  height: 16px;
  width: 100%;
  margin-bottom: ${({ theme }) => theme.spacing.xs};
`

const SkeletonOverviewShort = styled(SkeletonBase)`
  height: 16px;
  width: 70%;
  margin-bottom: ${({ theme }) => theme.spacing.sm};
`

const SkeletonInfo = styled(SkeletonBase)`
  height: 14px;
  width: 60%;
`

export const MovieCardSkeleton = () => {
  return (
    <SkeletonCard>
      <SkeletonPoster />
      <SkeletonContent>
        <SkeletonTitle />
        <SkeletonOverview />
        <SkeletonOverview />
        <SkeletonOverviewShort />
        <SkeletonInfo />
      </SkeletonContent>
    </SkeletonCard>
  )
}
