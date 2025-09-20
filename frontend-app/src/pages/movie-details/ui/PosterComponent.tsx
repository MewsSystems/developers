import { Box } from "@chakra-ui/react";
import type { DetailsProps } from "../types";

export function PosterComponent({
  detailsProps,
}: {
  detailsProps: DetailsProps;
}) {
  return (
    detailsProps.poster_img && (
      <Box>
        <img src={detailsProps.poster_img} />
      </Box>
    )
  );
}
