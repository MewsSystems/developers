import { useNavigate } from "@tanstack/react-router";
import { Flex, Box, Text } from "@chakra-ui/react";
import { WithFallbackImage } from "@/shared/ui/WithFallbackImage";
import { HorizontalItem } from "@/shared/ui/HorizontalItem";
import type { RecommendationDetail } from "@/features/recommendation/types";

export function RecommendationsComponent({
  recommendations,
}: {
  recommendations: RecommendationDetail[];
}) {
  const navigate = useNavigate();

  return (
    <Flex maxW="100%">
      {recommendations.map((c) => {
        return (
          <Box
            key={c.id + "_box"}
            onClick={() => {
              navigate({ to: `/movies/${c.id}` });
            }}
            cursor={"pointer"}
          >
            <HorizontalItem key={c.id} width={c.backdrop.width}>
              <WithFallbackImage
                width={c.backdrop.width}
                src={c.backdrop.imgSrc}
              />
            </HorizontalItem>
            <Box p="2">
              <Text>{c.title}</Text>
              <Text>{c.voteAverage}</Text>
            </Box>
          </Box>
        );
      })}
    </Flex>
  );
}
