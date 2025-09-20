import { Flex, Box, Container, Text } from "@chakra-ui/react";
import type { Crew, MovieCredits } from "@/entities/movie/types";
import { formatList } from "@/shared/lib/utils";
import { usePreferredLanguage } from "@uidotdev/usehooks";

export function Credits({ credits }: { credits?: MovieCredits }) {
  const filterBy = "Director";
  const language = usePreferredLanguage();
  const crewDirectors = crewEntries(filterBy, credits?.crew);

  return (
    <Container>
      <Flex direction="column" alignItems="left" w="100%">
        {crewDirectors.map(({ name, jobs }) => {
          return (
            <Box key={name}>
              <Text textStyle="xl">{name}</Text>
              <Text>{formatList(jobs, language)}</Text>
            </Box>
          );
        })}
        <Box></Box>
      </Flex>
    </Container>
  );
}

function crewEntries(filterBy: string, crew?: Crew[]) {
  const crewMap: Record<string, string[]> = (crew ?? []).reduce(
    (crewMap: Record<string, string[]>, crewItem) => {
      crewMap[crewItem.name] = crewMap[crewItem.name] ?? [];
      crewMap[crewItem.name].push(crewItem.job);
      return crewMap;
    },
    {}
  );

  return Object.entries(crewMap)
    .filter((entry) => entry[1].includes(filterBy))
    .map(([name, jobs]) => ({ name, jobs }));
}
