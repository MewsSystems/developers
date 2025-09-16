import type { DetailsProps } from "@/pages/movie-details/types";
import { formatDollar } from '@/shared/lib/utils';
import { Box, Flex, Text } from "@chakra-ui/react";
import { getLangNameFromCode } from "language-name-map";
import { HomepageComponent } from "@/pages/movie-details/ui/Info/HomePageLink";
import { KeywordsComponent } from "@/pages/movie-details/ui/Info/KeywordsComponent";
import { IMDBLink } from "./ImdbLink";

export function Info({ detailsProps }: { detailsProps: DetailsProps }) {
    return (<Flex direction={"column"} gap="4">
        <Flex gap="5">
            <IMDBLink imdb_id={detailsProps.movie.imdb_id} />
            <HomepageComponent homepage={detailsProps.movie.homepage} />
        </Flex>
        <Box>
            <Text textStyle={"lg"}>Status</Text>
            <Text >{detailsProps.movie.status}</Text>
        </Box>
        <Box>
            <Text textStyle={"lg"}>Original Language</Text>
            <Text >{getLangNameFromCode(detailsProps.movie.original_language)?.name}</Text>
        </Box>
        <Box>
            <Text textStyle={"lg"}>Budget</Text>
            <Text >{formatDollar(detailsProps.movie.budget)}</Text>
        </Box>
        <Box>
            <Text textStyle={"lg"}>Revenue</Text>
            <Text >{formatDollar(detailsProps.movie.revenue)}</Text>
        </Box>
        <Box>
            <Text textStyle={"lg"}>Keywords</Text>
            <KeywordsComponent detailsProps={detailsProps} />
        </Box>
    </Flex>)
}

