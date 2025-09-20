import { Image, Flex, Box, Text } from "@chakra-ui/react";
import { HorizontalItem } from "@/shared/ui/HorizontalItem";
import type { CastImg } from "@/features/topBilledCast/types";

export function CastComponent({ castImgs }: { castImgs: CastImg[] }) {
  return (
    <Flex maxW="100%">
      {castImgs.map((c) => {
        return (
          <HorizontalItem key={c.id} width={c.width}>
            <Image src={c.img} />
            <Box p="2">
              <Text>{c.name}</Text>
              <Text>{c.character}</Text>
            </Box>
          </HorizontalItem>
        );
      })}
    </Flex>
  );
}
