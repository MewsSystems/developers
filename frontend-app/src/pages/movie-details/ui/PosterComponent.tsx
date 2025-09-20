import { Box } from "@chakra-ui/react";

export function PosterComponent({ poster_img }: { poster_img: string }) {
  return (
    poster_img && (
      <Box>
        <img src={poster_img} />
      </Box>
    )
  );
}
