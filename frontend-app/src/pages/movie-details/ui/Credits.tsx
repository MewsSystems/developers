import { Flex, Box, Container, Text } from "@chakra-ui/react"
import type { DetailsProps } from '@/pages/movie-details/types';

export function Credits({ detailsProps }: { detailsProps: DetailsProps }) {
    const crewMap: Record<string, string[]> = (detailsProps.movie.credits?.crew ?? []).reduce((crewMap: Record<string, string[]>, crewItem) => {
        crewMap[crewItem.name] = crewMap[crewItem.name] ?? [];
        crewMap[crewItem.name].push(crewItem.job);
        return crewMap;
    }, {});

    const crewsFiltered = Object.entries(crewMap).filter(entry => entry[1].includes("Director"));

    return (
        <Container>
            <Flex direction="column" alignItems="left" w="100%">
                {(crewsFiltered).map(c => {
                    return (
                        <Box key={c[0]}>
                            <Text textStyle="xl">{c[0]}</Text>
                            <Text>{c[1].join(", ")}</Text>
                        </Box>
                    )
                })}
                <Box>
                </Box>
            </Flex>
        </Container>
    )
}
