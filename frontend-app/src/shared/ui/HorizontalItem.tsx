import { Box, Flex } from "@chakra-ui/react";

export function HorizontalItem({ children, width }: React.PropsWithChildren & { width: string }) {
    return <Flex direction="column">
        <Box m="2" rounded="lg" overflow="hidden" width={width}>
            {children}
        </Box>
    </Flex>

}

