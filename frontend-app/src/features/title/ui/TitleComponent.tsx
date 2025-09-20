import {
  Flex,
  Container,
  HStack,
  Text,
  StackSeparator,
} from "@chakra-ui/react";
import type { MovieTitle } from "@/features/title/types";

export function TitleComponent({ title }: { title: MovieTitle }) {
  return (
    <Container>
      <Flex alignContent="left" w="100%" direction="column">
        <Text textStyle="4xl">
          {title.title}({title.releaseDateYearLocale})
        </Text>
        <HStack separator={<StackSeparator />}>
          <Text>
            {title.releaseDateLocale}({title.countriesOrigin})
          </Text>
          <Text>{title.genres}</Text>
          <Text>{title.duration}</Text>
        </HStack>
      </Flex>
    </Container>
  );
}
