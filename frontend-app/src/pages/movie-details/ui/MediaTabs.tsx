import { Image, Flex, Box } from "@chakra-ui/react";
import { Tabs } from "@chakra-ui/react";
import { HorizontalItem } from "./HorizontalItem";
import type { MovieMedia } from "@/pages/movie-details/types";

import { FaRegPlayCircle } from "react-icons/fa";

export function MediaTabs({ movieMedia }: { movieMedia: MovieMedia }) {
  const tabs = [
    {
      name: `Videos(${movieMedia.totalVideos})`,
      total: movieMedia.totalVideos,
      value: "videos",
    },
    {
      name: `Backdrops(${movieMedia.totalBackdrops})`,
      total: movieMedia.totalBackdrops,
      value: "backdrops",
    },
    {
      name: `Posters(${movieMedia.totalPosters})`,
      total: movieMedia.totalPosters,
      value: "posters",
    },
  ];

  const tabsAvailable = tabs.filter((tab) => tab.total > 0);
  if (tabsAvailable.length == 0) {
    return <></>;
  }

  return (
    <Tabs.Root defaultValue={tabsAvailable[0].value}>
      <Tabs.List>
        {tabsAvailable.map((tab) => {
          return <Tabs.Trigger value={tab.value}>{tab.name}</Tabs.Trigger>;
        })}
      </Tabs.List>
      <Tabs.Content value="videos">
        <Flex>
          {movieMedia.videosMedia.map((video) => {
            return (
              <HorizontalItem key={video.id} width={video.width}>
                <a target="_blank" href={video.youtubeUrl}>
                  <Box
                    position="relative"
                    width={video.width}
                    height={video.height}
                  >
                    <Box position="absolute" top="0" left="0">
                      <Image src={video.youtubeImgSrc} />
                    </Box>
                    <Box
                      position="absolute"
                      top="50%"
                      left="50%"
                      transform="translate(-50%, -50%)"
                    >
                      <FaRegPlayCircle size="40" />
                    </Box>
                  </Box>
                </a>
              </HorizontalItem>
            );
          })}
        </Flex>
      </Tabs.Content>
      <Tabs.Content value="backdrops">
        <Flex>
          {movieMedia.backdropsMedia.map((backdrop) => {
            return (
              <HorizontalItem key={backdrop.file_path} width={backdrop.width}>
                <Image src={backdrop.imgSrc} />
              </HorizontalItem>
            );
          })}
        </Flex>
      </Tabs.Content>
      <Tabs.Content value="posters">
        <Flex>
          {movieMedia.postersMedia.map((poster) => {
            return (
              <HorizontalItem key={poster.file_path} width={poster.width}>
                <Image src={poster.imgSrc} />
              </HorizontalItem>
            );
          })}
        </Flex>
      </Tabs.Content>
    </Tabs.Root>
  );
}
