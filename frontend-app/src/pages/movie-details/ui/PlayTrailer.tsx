import type { DetailsProps } from "@/pages/movie-details/types";
import { Box, Flex, Text } from "@chakra-ui/react";
import { FaPlay } from "react-icons/fa";

export function PlayTrailer({ detailsProps }: { detailsProps: DetailsProps }) {
    const videoTrailer = detailsProps.movie.videos.results.find(video => video.type == "Trailer")
    if (videoTrailer) {
        return (<Box marginTop="8px">
            <a target="_blank" href={`https://www.youtube.com/watch?v=${videoTrailer.key}`}>
                <Flex alignContent="center" alignItems="center" gap="2">
                    <FaPlay color="black" />
                    <Text color="black">Play Trailler</Text>
                </Flex>
            </a>
        </Box>)
    }
    return <></>
}

