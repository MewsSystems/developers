import type { CrewDirectorDetails } from "@/features/crewDirectors/types";
import { Flex, Box, Container, Text } from "@chakra-ui/react";

export function Credits({
  crewDirectors,
}: {
  crewDirectors: CrewDirectorDetails[];
}) {
  return (
    <Container>
      <Flex direction="column" alignItems="left" w="100%">
        {crewDirectors.map(({ name, jobs }) => {
          return (
            <Box key={name}>
              <Text textStyle="xl">{name}</Text>
              <Text>{jobs}</Text>
            </Box>
          );
        })}
        <Box></Box>
      </Flex>
    </Container>
  );
}
