import { Flex, Container, Text } from "@chakra-ui/react";

export function OverviewComponent({ overview }: { overview: string }) {
    return (
        <Container>
            <Flex direction="column" alignItems="left" w="100%">
                <Text textStyle="xl">Overview</Text>
                <Text>{overview}</Text>
            </Flex>
        </Container>
    )
}

