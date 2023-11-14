import { apiConfig } from "@/domain/remote/config";
import { Text } from "@/styles/base/text";
import { DetailsContainer, DetailsWrapper } from "@/styles/components/details";
import { Actor } from "@/types/actor";
import { Image } from "@/types/image";
import { Movie } from "@/types/movie";
import { FC } from "react";
import { ActorsList } from "./actors-list";
import { Title } from "@/styles/base/title";
import { Stack } from "@/styles/base/stack";
import { PostersList } from "./posters-list";

type Props = {
  movie: Movie;
  actors: Actor[];
  images: Image[];
};

export const Details: FC<Props> = (props: Props) => {
  const { movie, actors, images, coverImage } = useDetails(props);

  return (
    <DetailsWrapper $bgImage={coverImage}>
      <DetailsContainer>
        <Stack $gap="xl">
          <Title $fs={48} $lh={48}>
            {movie.title}
          </Title>
          <Text $size="lg">
            ⭐️ {movie?.vote_average} | {movie?.vote_count}
          </Text>
          <Text $size="lg" $lh="xl">
            {movie?.overview}
          </Text>
        </Stack>
        <Stack>
          <ActorsList actors={actors} />
          <PostersList images={images} />
        </Stack>
      </DetailsContainer>
    </DetailsWrapper>
  );
};

function useDetails({ movie, actors, images }: Props) {
  const coverImage = apiConfig.coverImage(
    movie?.backdrop_path || movie?.poster_path
  );

  return { movie, actors, images, coverImage };
}
