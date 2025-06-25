import styled from "styled-components"

export const SkeletonBase = styled.div`
  background: linear-gradient(
    to right,
    ${({ theme }) => theme.colors.surface} 0%,
    ${({ theme }) => theme.colors.surfaceHover} 20%,
    ${({ theme }) => theme.colors.surface} 40%,
    ${({ theme }) => theme.colors.surface} 100%
  );
  background-size: 800px 104px;
  animation: ${({ theme }) => theme.animations.shimmer} 1.5s linear infinite;
  border-radius: ${({ theme }) => theme.borderRadius.base};
`

export const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: ${({ theme }) => theme.spacing.xl};
`

export const Header = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: ${({ theme }) => theme.spacing.xl};
  margin-bottom: ${({ theme }) => theme.spacing.xl};

  @media (max-width: ${({ theme }) => theme.breakpoints.md}) {
    grid-template-columns: 1fr;
    gap: ${({ theme }) => theme.spacing.lg};
  }
`

export const PosterContainer = styled.div`
  position: relative;
  aspect-ratio: 2/3;
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  overflow: hidden;
  box-shadow: ${({ theme }) => theme.shadows.lg};
`

export const PosterSkeleton = styled(SkeletonBase)`
  width: 100%;
  height: 100%;
`

export const Info = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing.md};
`

export const TitleSkeleton = styled(SkeletonBase)`
  height: 48px;
  width: 70%;
  margin-bottom: ${({ theme }) => theme.spacing.sm};
`

export const MetaInfo = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.lg};
  margin-bottom: ${({ theme }) => theme.spacing.md};
`

export const MetaSkeleton = styled(SkeletonBase)`
  height: 24px;
  width: 80px;
`

export const GenreList = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.sm};
  margin-bottom: ${({ theme }) => theme.spacing.lg};
`

export const GenreSkeleton = styled(SkeletonBase)`
  height: 32px;
  width: 80px;
  border-radius: ${({ theme }) => theme.borderRadius.full};
`

export const OverviewSkeleton = styled(SkeletonBase)`
  height: 20px;
  margin-bottom: ${({ theme }) => theme.spacing.sm};
  
  &:last-child {
    width: 75%;
  }
`

export const SectionTitle = styled(SkeletonBase)`
  height: 32px;
  width: 200px;
  margin-bottom: ${({ theme }) => theme.spacing.md};
  margin-top: ${({ theme }) => theme.spacing.xl};
`

export const ProductionList = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.md};
`

export const ProductionSkeleton = styled(SkeletonBase)`
  height: 32px;
  width: 120px;
`
