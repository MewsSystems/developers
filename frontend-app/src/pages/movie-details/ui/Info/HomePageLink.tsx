import { Box, Link, Text } from "@chakra-ui/react";
import { CiLink } from "react-icons/ci";

export function HomepageComponent({ homepage }: { homepage: string }) {
    return <Box>
        <Link href={homepage} target="_blank">
            <Text textStyle="3xl">
                <CiLink />
            </Text>
        </Link>
    </Box>

}