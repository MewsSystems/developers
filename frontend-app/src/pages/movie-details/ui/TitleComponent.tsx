import { Flex, Container, HStack, Text, StackSeparator } from "@chakra-ui/react"
import { toDurationFormat, toLocaleDate, toLocaleYear } from '@/shared/lib/utils';
import { usePreferredLanguage } from '@uidotdev/usehooks';
import type { DetailsProps } from '@/pages/movie-details/types';


export function TitleComponent({ detailsProps }: { detailsProps: DetailsProps }) {
    const language = usePreferredLanguage();
    const movie = detailsProps.movie
    const countriesOrigin = movie.origin_country.map(c => c).join(", ");
    const genres = movie.genres.map(g => g.name).join(", ");
    const duration = toDurationFormat(movie.runtime);
    const releaseDate = toLocaleDate(movie.release_date, language);
    const releaseDateYear = toLocaleYear(movie.release_date, language);
    return (
        <Container>
            <Flex alignContent="left" w="100%" direction="column">
                <Text textStyle="4xl">{movie.title}({releaseDateYear})</Text>
                <HStack separator={<StackSeparator />}>
                    <Text>{releaseDate}({countriesOrigin})</Text>
                    <Text>{genres}</Text>
                    <Text>{duration}</Text>
                </HStack>
            </Flex>
        </Container>
    )
}