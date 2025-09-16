import { Box, Tag, Text } from "@chakra-ui/react"
import type { DetailsProps } from "@/pages/movie-details/types"

export function KeywordsComponent({ detailsProps }: { detailsProps: DetailsProps }) {
    return <Box>
        {detailsProps.movie.keywords.keywords.map(k => {
            return (<Tag.Root m="2" key={k.id}>
                <Tag.Label p="1"><Text textStyle="xs">{k.name}</Text></Tag.Label>
            </Tag.Root>)
        })}
    </Box>
}

