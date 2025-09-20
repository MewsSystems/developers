import { Box, Tag, Text } from "@chakra-ui/react";
import type { Keyword } from "@/entities/movie/types";

export function KeywordsComponent({ keywords }: { keywords: Keyword[] }) {
  return (
    <Box>
      {keywords.map((k) => {
        return (
          <Tag.Root m="2" key={k.id}>
            <Tag.Label p="1">
              <Text textStyle="xs">{k.name}</Text>
            </Tag.Label>
          </Tag.Root>
        );
      })}
    </Box>
  );
}
