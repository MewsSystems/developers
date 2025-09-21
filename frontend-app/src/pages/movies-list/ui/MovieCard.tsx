import { Text, Flex, GridItem, Grid, HStack } from "@chakra-ui/react";
import type { MovieCardItem } from "../types";
import { Link } from "@tanstack/react-router";
import { FaHeart } from "react-icons/fa";
import { WithFallbackImage } from "@/shared/ui/WithFallbackImage";

export function MovieCard({
  movieCardItem,
  favoritesMap,
}: {
  movieCardItem: MovieCardItem;
  favoritesMap: Map<number, boolean>;
}) {
  const isFavorite = favoritesMap.get(movieCardItem.movie.id);
  return (
    <>
      <GridItem
        borderColor={"black"}
        borderWidth={"1px"}
        rounded="md"
        shadow={"lg"}
      >
        <Grid templateColumns="200px 8fr">
          <GridItem>
            <Link
              to="/moviedetails/$movieId"
              params={{ movieId: movieCardItem.movie.id.toString() }}
            >
              <WithFallbackImage
                src={movieCardItem.poster_img ?? ""}
                width="100px"
              />
            </Link>
          </GridItem>
          <GridItem p="4">
            <Link
              to="/moviedetails/$movieId"
              params={{ movieId: movieCardItem.movie.id.toString() }}
            >
              <Flex direction="column" gap="4">
                <Flex direction="column" gap="1">
                  <HStack gap="4" alignItems={"center"}>
                    <Text textStyle="4xl">{movieCardItem.movie.title}</Text>
                    {isFavorite ? <FaHeart color="red" /> : ""}
                  </HStack>
                  <Text textStyle={"sm"}>
                    {movieCardItem.release_date_locale}
                  </Text>
                </Flex>
                <Text>{movieCardItem.movie.overview}</Text>
              </Flex>
            </Link>
          </GridItem>
        </Grid>
      </GridItem>
    </>
  );
}
