import { Box } from "@chakra-ui/react";
import type { DetailsProps } from "../types";

export function PosterComponent({ detailsProps }: { detailsProps: DetailsProps }) {
    return detailsProps.movie.poster_path && <Box><img src={detailsProps.boundIncludeConfiguration(detailsProps.movie.poster_path, 3)} /></Box>
}
