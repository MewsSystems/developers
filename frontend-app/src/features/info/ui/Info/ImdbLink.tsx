import { Box, Text, Link } from "@chakra-ui/react"
import { FaImdb } from "react-icons/fa";

export function IMDBLink({ imdbLink }: { imdbLink?: string }) {
    if (!imdbLink) {
        return <></>;
    }
    return <Box>
        <Link href={`${imdbLink}`} target="_blank">
            <Text textStyle="3xl">
                <FaImdb />
            </Text>
        </Link>
    </Box>
}