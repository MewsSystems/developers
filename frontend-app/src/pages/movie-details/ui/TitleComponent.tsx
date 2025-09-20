import {
  Flex,
  Container,
  HStack,
  Text,
  StackSeparator,
} from "@chakra-ui/react";
import type { DetailsProps } from "@/pages/movie-details/types";

export function TitleComponent({
  detailsProps,
}: {
  detailsProps: DetailsProps;
}) {
  const movie = detailsProps.movie;
  return (
    <Container>
      <Flex alignContent="left" w="100%" direction="column">
        <Text textStyle="4xl">
          {movie.title}({detailsProps.releaseDateYearLocale})
        </Text>
        <HStack separator={<StackSeparator />}>
          <Text>
            {detailsProps.releaseDateLocale}({detailsProps.countriesOrigin})
          </Text>
          <Text>{detailsProps.genres}</Text>
          <Text>{detailsProps.duration}</Text>
        </HStack>
      </Flex>
    </Container>
  );
}
