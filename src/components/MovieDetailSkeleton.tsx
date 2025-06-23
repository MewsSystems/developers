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

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: ${({ theme }) => theme.spacing.xl};
`

const Header = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: ${({ theme }) => theme.spacing.xl};
  margin-bottom: ${({ theme }) => theme.spacing.xl};

  @media (max-width: ${({ theme }) => theme.breakpoints.md}) {
    grid-template-columns: 1fr;
    gap: ${({ theme }) => theme.spacing.lg};
  }
`

const PosterContainer = styled.div`
  position: relative;
  aspect-ratio: 2/3;
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  overflow: hidden;
  box-shadow: ${({ theme }) => theme.shadows.lg};
`

const PosterSkeleton = styled(SkeletonBase)`
  width: 100%;
  height: 100%;
`

const Info = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing.md};
`

const TitleSkeleton = styled(SkeletonBase)`
  height: 48px;
  width: 70%;
  margin-bottom: ${({ theme }) => theme.spacing.sm};
`

const MetaInfo = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.lg};
  margin-bottom: ${({ theme }) => theme.spacing.md};
`

const MetaSkeleton = styled(SkeletonBase)`
  height: 24px;
  width: 80px;
`

const GenreList = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.sm};
  margin-bottom: ${({ theme }) => theme.spacing.lg};
`

const GenreSkeleton = styled(SkeletonBase)`
  height: 32px;
  width: 80px;
  border-radius: ${({ theme }) => theme.borderRadius.full};
`

const OverviewSkeleton = styled(SkeletonBase)`
  height: 20px;
  margin-bottom: ${({ theme }) => theme.spacing.sm};
  
  &:last-child {
    width: 75%;
  }
`

const SectionTitle = styled(SkeletonBase)`
  height: 32px;
  width: 200px;
  margin-bottom: ${({ theme }) => theme.spacing.md};
  margin-top: ${({ theme }) => theme.spacing.xl};
`

const ProductionList = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ theme }) => theme.spacing.md};
`

const ProductionSkeleton = styled(SkeletonBase)`
  height: 32px;
  width: 120px;
`

export const MovieDetailSkeleton = () => {
  return (
    <Container>
      <Header>
        <PosterContainer>
          <PosterSkeleton />
        </PosterContainer>

        <Info>
          <TitleSkeleton />

          <MetaInfo>
            <MetaSkeleton />
            <MetaSkeleton />
            <MetaSkeleton />
            <MetaSkeleton />
          </MetaInfo>

          <GenreList>
            <GenreSkeleton />
            <GenreSkeleton />
            <GenreSkeleton />
          </GenreList>

          <div>
            <OverviewSkeleton />
            <OverviewSkeleton />
            <OverviewSkeleton />
            <OverviewSkeleton />
            <OverviewSkeleton />
          </div>
        </Info>
      </Header>

      <div>
        <SectionTitle />
        <ProductionList>
          <ProductionSkeleton />
          <ProductionSkeleton />
          <ProductionSkeleton />
        </ProductionList>
      </div>
    </Container>
  )
}
