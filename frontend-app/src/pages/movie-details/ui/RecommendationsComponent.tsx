import { useNavigate } from "@tanstack/react-router";
import { Flex, Box, Text } from "@chakra-ui/react";
import { parseBackdropToImgPath, parseWidth } from "@/shared/lib/utils";
import { WithFallbackImage } from "@/shared/ui/WithFallbackImage";
import { HorizontalItem } from "@/pages/movie-details/ui/HorizontalItem";
import type { DetailsProps } from "@/pages/movie-details/types";

export function RecommendationsComponent({
  detailsProps,
}: {
  detailsProps: DetailsProps;
}) {
  const navigate = useNavigate();
  const backdropSizePosition = 0;
  const backdropWidth = parseWidth(
    detailsProps.configuration.images.backdrop_sizes[backdropSizePosition]
  );

  const recommendations = detailsProps.movie.recommendations.results;
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
            <HorizontalItem key={c.id} width={backdropWidth}>
              <WithFallbackImage
                width={backdropWidth}
                src={parseBackdropToImgPath(
                  detailsProps.configuration.images,
                  c.backdrop_path,
                  backdropSizePosition
                )}
              />
            </HorizontalItem>
            <Box p="2">
              <Text>{c.title}</Text>
              <Text>{(c.vote_average * 10).toFixed(0)}%</Text>
            </Box>
          </Box>
        );
      })}
    </Flex>
  );
}
