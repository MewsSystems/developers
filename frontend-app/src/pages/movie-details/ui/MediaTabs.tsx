import { Image, Flex, Box } from "@chakra-ui/react"
import { createYoutubeImgUrl, parseBackdropToImgPath, parsePosterToImgPath, parseWidth, YOUTUBESIZES } from '@/shared/lib/utils';
import { Tabs } from "@chakra-ui/react"
import { slice } from 'lodash';
import { HorizontalItem } from "./HorizontalItem";
import type { DetailsProps } from "../types";

import { FaRegPlayCircle } from "react-icons/fa";

export function MediaTabs({ detailsProps }: { detailsProps: DetailsProps }) {
    if (!detailsProps.images) {
        return <></>
    }
    const MAX_SHOW = 2
    const videos = detailsProps.movie.videos.results.filter(v => v.site === "YouTube");;
    const shownVideos = slice(videos, 0, MAX_SHOW);

    const backdrops = detailsProps.images.backdrops;
    const shownbackdrops = slice(backdrops, 0, MAX_SHOW);
    const backdropSizePosition = 1;
    const backdropWidth = "480px";

    const posters = detailsProps.images.posters;
    const shownposters = slice(posters, 0, MAX_SHOW);
    const posterSizePosition = 2;
    const posterWidth = parseWidth(detailsProps.configuration.images.poster_sizes[posterSizePosition]);
    return (<Tabs.Root defaultValue="videos">
        <Tabs.List>
            <Tabs.Trigger value="videos">
                Videos({detailsProps.movie.videos.results.length})
            </Tabs.Trigger>
            <Tabs.Trigger value="backdrops">
                Backdrops({backdrops.length})
            </Tabs.Trigger>
            <Tabs.Trigger value="posters">
                Posters({detailsProps.images.posters.length})
            </Tabs.Trigger>
        </Tabs.List>
        <Tabs.Content value="videos">
            <Flex>
                {shownVideos.map(video => {
                    return (
                        <HorizontalItem key={video.id} width="480px">
                            <a target="_blank" href={`https://www.youtube.com/watch?v=${video.key}`}>
                                <Box position="relative" width="480px" height="400px">
                                    <Box position="absolute" top="0" left="0">
                                        <Image src={(createYoutubeImgUrl(video.key, YOUTUBESIZES.HQ))} />
                                    </Box>
                                    <Box position="absolute" top="50%" left="50%" transform="translate(-50%, -50%)">
                                        <FaRegPlayCircle size="40" />
                                    </Box>
                                </Box>
                            </a>
                        </HorizontalItem>
                    )
                })}
            </Flex>
        </Tabs.Content>
        <Tabs.Content value="backdrops">
            <Flex>
                {shownbackdrops.map(backdrop => {
                    return (
                        <HorizontalItem key={backdrop.file_path} width={backdropWidth}>
                            <Image src={parseBackdropToImgPath(detailsProps.configuration.images, backdrop.file_path, backdropSizePosition)} />
                        </HorizontalItem>
                    )
                })}
            </Flex>
        </Tabs.Content>
        <Tabs.Content value="posters">
            <Flex>
                {shownposters.map(poster => {
                    return (
                        <HorizontalItem key={poster.file_path} width={posterWidth}>
                            <Image src={parsePosterToImgPath(detailsProps.configuration.images, poster.file_path, posterSizePosition)} />
                        </HorizontalItem>
                    )
                })}
            </Flex>
        </Tabs.Content>
    </Tabs.Root>)
}