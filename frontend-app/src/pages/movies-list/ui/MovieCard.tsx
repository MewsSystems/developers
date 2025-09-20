import { Text, HStack, VStack, Card } from "@chakra-ui/react";

import { toLocaleDate } from "@/shared/lib/utils";
import { usePreferredLanguage } from "@uidotdev/usehooks";
import type { MovieCardItem } from "../types";

export function MovieCard({ movieCardItem }: { movieCardItem: MovieCardItem }) {
  const language = usePreferredLanguage();
  return (
    <Card.Root width="720px" variant={"subtle"} key={movieCardItem.movie.id}>
      <Card.Body gap="2">
        <HStack>
          <img src={movieCardItem.poster_img ?? ""} />
          <VStack>
            <Card.Title mb="2">
              <Text>
                {" "}
                <a href={`/moviedetails/${movieCardItem.movie.id}`}>
                  {movieCardItem.movie.title}
                </a>
              </Text>
              <Text>
                {toLocaleDate(movieCardItem.movie.release_date, language)}
              </Text>
            </Card.Title>
            <Card.Description>{movieCardItem.movie.overview}</Card.Description>
          </VStack>
        </HStack>
      </Card.Body>
    </Card.Root>
  );
}
