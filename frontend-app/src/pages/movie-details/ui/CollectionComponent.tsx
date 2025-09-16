import { Flex, Box, Container, Text } from "@chakra-ui/react"
import { parseBackdropToImgPath, formatList } from '@/shared/lib/utils';
import { OpacityBackgroundBox } from '@/shared/ui/OpacityBackground';
import type { DetailsProps } from '@/pages/movie-details/types';

export function CollectionComponent({ detailsProps }: { detailsProps: DetailsProps }) {
    if (!detailsProps.collection) {
        return <></>
    }
    const collection = detailsProps.collection;
    const backdropSizePosition = 6

    const backdrop_img_path = collection.backdrop_path ? parseBackdropToImgPath(detailsProps.configuration.images, collection.backdrop_path, backdropSizePosition) : "";
    const bgImage = `url(${backdrop_img_path})`
    const parts = formatList(collection.parts.map(({ title }) => title), detailsProps.language);
    return <Box rounded="lg" overflow="hidden" maxW="100%">
        <OpacityBackgroundBox bgImage={bgImage}>
            <Container p="5">
                <Flex direction={"column"}>
                    <Text textStyle={"4xl"}>Part of {collection.name}</Text>
                    <Text>Includes {parts}</Text>
                </Flex>
            </Container>
        </OpacityBackgroundBox>
    </Box>
}

