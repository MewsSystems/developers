import { Flex, Box, Container, Text } from "@chakra-ui/react"
import RatingChart from '@/shared/ui/RatingChart';
import { OpacityBackgroundBox } from '@/shared/ui/OpacityBackground';
import { MediaTabs } from '@/pages/movie-details/ui/MediaTabs';
import type { DetailsProps } from '@/pages/movie-details/types';
import { PosterComponent } from '@/pages/movie-details/ui/PosterComponent';
import { Info } from '@/pages/movie-details/ui/Info';
import { CastComponent } from '@/pages/movie-details/ui/CastComponent';
import { RecommendationsComponent } from "@/pages/movie-details/ui/RecommendationsComponent";
import { SocialTabs } from "@/pages/movie-details/ui/SocialTabs";
import { CollectionComponent } from "@/pages/movie-details/ui/CollectionComponent";
import { TaglineComponent } from "@/pages/movie-details/ui/TaglineComponent";
import { TitleComponent } from "@/pages/movie-details/ui/TitleComponent";
import { OverviewComponent } from "@/pages/movie-details/ui/OverviewComponent";
import { Credits } from "@/pages/movie-details/ui/Credits";
import { PlayTrailer } from "@/pages/movie-details/ui/PlayTrailer";
import { ToggleFavorite } from "@/pages/movie-details/ui/ToggleFavorite";
import { ToggleWatchList } from "@/pages/movie-details/ui/ToggleWatchList";

export function DetailsComponent({ detailsProps }: { detailsProps: DetailsProps }) {
    const backdrop_img_path = detailsProps.movie.backdrop_path ? detailsProps.boundIncludeConfiguration(detailsProps.movie.backdrop_path, 6) : "";
    const bgImage = `url(${backdrop_img_path})`


    return (<Container p="10">
        <Flex direction="column" gap="4">
            <OpacityBackgroundBox bgImage={bgImage}>
                <Box flexBasis="25%" >
                    <PosterComponent detailsProps={detailsProps} />
                </Box>
                <Box flexBasis="75%">
                    <Flex direction="column" gap="4">
                        <TitleComponent detailsProps={detailsProps} />
                        <Container>
                            <Flex>
                                <ToggleFavorite detailsProps={detailsProps} />
                                <ToggleWatchList detailsProps={detailsProps} />
                                <PlayTrailer detailsProps={detailsProps} />
                            </Flex>
                        </Container>
                        <TaglineComponent tagline={detailsProps.movie.tagline} />
                        <OverviewComponent overview={detailsProps.movie.overview} />
                        <Credits detailsProps={detailsProps} />
                        <Container height={"100px"}>
                            <RatingChart percent={Math.round(detailsProps.movie.vote_average * 10)} />
                        </Container>
                    </Flex>
                </Box>
            </OpacityBackgroundBox>
            <Flex gap="4">
                <Flex direction="column" flexBasis="75%" overflow="hidden">
                    {detailsProps.movie.credits.cast.length > 0 && <Box datatest-id="top_billed_cast" overflowX="auto" maxW={"100%"}>
                        <Text textStyle={"3xl"}>Top Billed Cast</Text>
                        <CastComponent detailsProps={detailsProps} />
                    </Box>}
                    <Box overflowX="auto" maxW={"100%"} datatest-id="media">
                        <MediaTabs detailsProps={detailsProps} />
                    </Box>
                    <Box overflowX="auto" maxW={"100%"} datatest-id="social">
                        <SocialTabs detailsProps={detailsProps} />
                    </Box>
                    <Box overflowX="auto" maxW={"100%"} datatest-id="collection">
                        <CollectionComponent detailsProps={detailsProps} />
                    </Box>
                    <Box datatest-id="recommendations" overflowX="auto" maxW={"100%"}>
                        <Text textStyle={"3xl"}>Recommendations</Text>
                        <RecommendationsComponent detailsProps={detailsProps} />
                    </Box>
                </Flex>
                <Flex flexBasis="25%">
                    <Info detailsProps={detailsProps} />
                </Flex>
            </Flex>
        </Flex>
    </Container>)
}