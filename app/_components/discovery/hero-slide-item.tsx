import { FC } from "react";
import { Movie } from "@/types/movie";
import {
  HeroSlideItemImage,
  HeroSlideItem as StyledHeroSlideItem,
} from "@/styles/components/hero-slide-item";
import { apiConfig } from "@/domain/remote/config";
import { Group } from "@/styles/base/group";
import { Stack } from "@/styles/base/stack";
import { Title } from "@/styles/base/title";
import { Text } from "@/styles/base/text";
import { PrimaryButton } from "@/styles/base/button";
import { useTranslations } from "next-intl";
import { useRouter } from "@/navigation";
import { paths } from "@/navigation/paths";

type Props = {
  movie: Movie;
  isActive: boolean;
};

export const HeroSlideItem: FC<Props> = (props) => {
  const { t, movie, isActive, coverImage, posterImage, handleNavigation } =
    useHeroSlideItem(props);

  return (
    <StyledHeroSlideItem $isActive={isActive} $bgImage={coverImage}>
      <Group $fullHeight $align="center" $justify="center" $gap="xl">
        <HeroSlideItemImage
          src={posterImage}
          width={400}
          height={600}
          alt={movie.title}
        />
        <Stack $gap="xl" $maw={720}>
          <Title $fs={48} $lh={56} $ta="center">
            {movie.title}
          </Title>
          <Text $size="md" $lh="lg" $ta="center">
            {movie.overview}
          </Text>
          <PrimaryButton onClick={handleNavigation}>
            {t("watchNowAction")}
          </PrimaryButton>
        </Stack>
      </Group>
    </StyledHeroSlideItem>
  );
};

function useHeroSlideItem({ movie, isActive }: Props) {
  const t = useTranslations("discovery");
  const { push } = useRouter();

  const coverImage = apiConfig.coverImage(
    movie.backdrop_path || movie.poster_path
  );
  const posterImage = apiConfig.posterImage(
    movie.poster_path || movie.backdrop_path
  );

  const handleNavigation = () => push(paths.movie(movie.id));

  return { t, movie, isActive, coverImage, posterImage, handleNavigation };
}
