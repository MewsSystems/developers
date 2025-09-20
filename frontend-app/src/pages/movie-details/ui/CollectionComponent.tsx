import { Flex, Box, Container, Text } from "@chakra-ui/react";
import { OpacityBackgroundBox } from "@/shared/ui/OpacityBackground";
import type { CollectionDetail } from "@/pages/movie-details/types";

export function CollectionComponent({
  collection,
}: {
  collection: CollectionDetail;
}) {
  return (
    <Box rounded="lg" overflow="hidden" maxW="100%">
      <OpacityBackgroundBox bgImage={collection.bgImage}>
        <Container p="5">
          <Flex direction={"column"}>
            <Text textStyle={"4xl"}>Part of {collection.name}</Text>
            <Text>Includes {collection.parts}</Text>
          </Flex>
        </Container>
      </OpacityBackgroundBox>
    </Box>
  );
}
