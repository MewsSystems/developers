import { slice } from "lodash";
import { Image, Flex, Box, Text } from "@chakra-ui/react";
import { parseProfileToImgPath, parseWidth } from "@/shared/lib/utils";
import type { DetailsProps } from "@/pages/movie-details/types";
import { HorizontalItem } from "@/pages/movie-details/ui/HorizontalItem";

const MAX_CAST = 9;

export function CastComponent({ detailsProps }: { detailsProps: DetailsProps }) {
    const cast = slice(detailsProps.movie.credits.cast, 0, MAX_CAST);
    const profileSizePosition = 1;
    const profileWidth = parseWidth(detailsProps.configuration.images.profile_sizes[profileSizePosition]);

    return (<Flex maxW="100%">
        {cast.map(c => {
            return <HorizontalItem key={c.id} width={profileWidth}>
                <Image src={parseProfileToImgPath(detailsProps.configuration.images, c.profile_path, profileSizePosition)} />
                <Box p="2">
                    <Text>{c.name}</Text>
                    <Text>{c.character}</Text>
                </Box>
            </HorizontalItem>
        })}
    </Flex>)
}
