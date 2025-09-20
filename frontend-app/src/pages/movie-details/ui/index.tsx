import { useParams } from "@tanstack/react-router";
import { useQueryMovieDetails } from "@/pages/movie-details/hooks/useQueryMovieDetails";
import { Flex, Box, Container, Text } from "@chakra-ui/react";
import RatingChart from "@/shared/ui/RatingChart";
import { OpacityBackgroundBox } from "@/shared/ui/OpacityBackground";
import { MediaTabs } from "@/features/media/ui/MediaTabs";
import type { DetailsProps } from "@/pages/movie-details/types";
import { PosterComponent } from "@/pages/movie-details/ui/PosterComponent";
import { Info } from "@/features/info/ui/Info";
import { CastComponent } from "@/features/topBilledCast/ui/CastComponent";
import { RecommendationsComponent } from "@/features/recommendation/ui/RecommendationsComponent";
import { SocialTabs } from "@/features/social/ui/SocialTabs";
import { CollectionComponent } from "@/features/collection/ui/CollectionComponent";
import { TaglineComponent } from "@/pages/movie-details/ui/TaglineComponent";
import { TitleComponent } from "@/features/title/ui/TitleComponent";
import { OverviewComponent } from "@/pages/movie-details/ui/OverviewComponent";
import { Credits } from "@/features/crewDirectors/ui/Credits";
import { PlayTrailer } from "@/features/videoTrailer/ui/PlayTrailer";
import { ToggleFavorite } from "@/pages/movie-details/ui/ToggleFavorite";
import { ToggleWatchList } from "@/pages/movie-details/ui/ToggleWatchList";

export function MovieDetailsRouteComponent() {
  const { movieId } = useParams({ from: "/_auth/moviedetails/$movieId" });
  const {
    data: details,
    isLoading,
    isError,
    error,
  } = useQueryMovieDetails({ movie_id: movieId });
  if (isLoading) {
    return <div>LOADING</div>;
  }
  if (isError) {
    console.error(error);
    return <Box>Sorry, error</Box>;
  }
  return (
    <div>{details?.movie && <DetailsComponent detailsProps={details} />}</div>
  );
}

function DetailsComponent({ detailsProps }: { detailsProps: DetailsProps }) {
  const bgImage = detailsProps.backdrop_img_url_css;

  return (
    <Container p="10">
      <Flex direction="column" gap="4">
        <OpacityBackgroundBox bgImage={bgImage}>
          <Box flexBasis="25%">
            <PosterComponent poster_img={detailsProps.poster_img} />
          </Box>
          <Box flexBasis="75%">
            <Flex direction="column" gap="4">
              <TitleComponent title={detailsProps.title} />
              <Container>
                <Flex>
                  <ToggleFavorite
                    movieId={detailsProps.movie.id}
                    favorite={detailsProps.movie.account_states.favorite}
                  />
                  <ToggleWatchList
                    movieId={detailsProps.movie.id}
                    watchlist={detailsProps.movie.account_states.watchlist}
                  />
                  <PlayTrailer
                    videoYoutubeTrailer={detailsProps.videoYoutubeTrailer}
                  />
                </Flex>
              </Container>
              <TaglineComponent tagline={detailsProps.movie.tagline} />
              <OverviewComponent overview={detailsProps.movie.overview} />
              <Credits crewDirectors={detailsProps.crewDirectors} />
              <Container height={"100px"}>
                <RatingChart
                  percent={Math.round(detailsProps.movie.vote_average * 10)}
                />
              </Container>
            </Flex>
          </Box>
        </OpacityBackgroundBox>
        <Flex gap="4">
          <Flex direction="column" flexBasis="75%" overflow="hidden">
            {detailsProps.castImgs.length > 0 && (
              <Box datatest-id="top_billed_cast" overflowX="auto" maxW={"100%"}>
                <Text textStyle={"3xl"}>Top Billed Cast</Text>
                <CastComponent castImgs={detailsProps.castImgs} />
              </Box>
            )}
            <Box overflowX="auto" maxW={"100%"} datatest-id="media">
              <MediaTabs movieMedia={detailsProps.media} />
            </Box>
            {detailsProps.movie.reviews.results.length > 0 && (
              <Box overflowX="auto" maxW={"100%"} datatest-id="social">
                <SocialTabs
                  totalReviews={detailsProps.movie.reviews.results.length}
                  reviews={detailsProps.reviews}
                />
              </Box>
            )}
            {detailsProps.collection && (
              <Box overflowX="auto" maxW={"100%"} datatest-id="collection">
                <CollectionComponent collection={detailsProps.collection} />
              </Box>
            )}
            <Box datatest-id="recommendations" overflowX="auto" maxW={"100%"}>
              <Text textStyle={"3xl"}>Recommendations</Text>
              <RecommendationsComponent
                recommendations={detailsProps.recommendations}
              />
            </Box>
          </Flex>
          <Flex flexBasis="25%">
            <Info info={detailsProps.info} />
          </Flex>
        </Flex>
      </Flex>
    </Container>
  );
}
