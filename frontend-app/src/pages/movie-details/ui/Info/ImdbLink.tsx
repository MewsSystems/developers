import { Box, Text, Link } from "@chakra-ui/react"
import { FaImdb } from "react-icons/fa";

export function IMDBLink({ imdb_id }: { imdb_id?: string }) {
    if (!imdb_id) {
        return <></>;
    }
    return <Box>
        <Link href={`https://www.imdb.com/title/${imdb_id}`} target="_blank">
            <Text textStyle="3xl">
                <FaImdb />
            </Text>
        </Link>
    </Box>
}