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
import { TvShow } from "@/types/tv-show";

type Props = {
  item: Movie | TvShow | (Movie & TvShow);
  actors: Actor[];
  images: Image[];
};

export const Details: FC<Props> = (props: Props) => {
  const { item, title, actors, images, coverImage } = useDetails(props);

  return (
    <DetailsWrapper $bgImage={coverImage}>
      <DetailsContainer>
        <Stack $gap="xl">
          <Title $fs={48} $lh={48}>
            {title}
          </Title>
          <Text $size="lg">
            ⭐️ {item?.vote_average} | {item?.vote_count}
          </Text>
          <Text $size="lg" $lh="xl">
            {item?.overview}
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

function useDetails({ item, actors, images }: Props) {
  const coverImage = apiConfig.coverImage(
    item?.backdrop_path || item?.poster_path
  );

  const title = (item as Movie).title ?? (item as TvShow).name ?? "";

  return { item, title, actors, images, coverImage };
}
