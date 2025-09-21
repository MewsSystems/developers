import { Container, Text } from "@chakra-ui/react"

export function TaglineComponent({ tagline }: { tagline: string }) {
    if (tagline) {
        return (<Container>
            <Text>{tagline}</Text>
        </Container>)
    }
    return <></>
}

