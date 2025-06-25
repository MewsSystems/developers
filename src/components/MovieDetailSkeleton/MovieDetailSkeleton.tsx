import {
  Container,
  GenreList,
  GenreSkeleton,
  Header,
  Info,
  MetaInfo,
  MetaSkeleton,
  OverviewSkeleton,
  PosterContainer,
  PosterSkeleton,
  ProductionList,
  ProductionSkeleton,
  SectionTitle,
  TitleSkeleton,
} from "./MovieDetailSkeleton.styles"

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
