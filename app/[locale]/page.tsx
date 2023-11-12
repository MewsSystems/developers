import { Stack } from "@/styles/base/stack";
import { Text } from "@/styles/base/text";
import { HeroTitle, LandingHeroSection } from "@/styles/pages/home";
import { IconVideo } from "@tabler/icons-react";
import { useTranslations } from "next-intl";

export default function HomePage() {
  const { t } = useHomePage();

  return (
    <LandingHeroSection>
      <HeroTitle $variant="h1" $fs={128} $lh={140}>
        {t.rich("title", { br: () => <br /> })}
      </HeroTitle>
      <Stack $gap="xl">
        <Stack $gap="xs">
          <Text $size="xl" $fw={700}>
            {t("stats.moviesCount")}
          </Text>
          <Text $c="textSecondary" $ta="center">
            {t("stats.moviesCountLabel")}
          </Text>
        </Stack>
        <Stack $align="center">
          <IconVideo size={36} />
          <Text $c="textSecondary" $ta="center">
            {t("stats.topAndPopularLabel")}
          </Text>
        </Stack>
        <Stack $gap="xs">
          <Text $size="xl" $fw={700}>
            {t("stats.showsCount")}
          </Text>
          <Text $c="textSecondary" $ta="center">
            {t("stats.showsCountLabel")}
          </Text>
        </Stack>
      </Stack>
    </LandingHeroSection>
  );
}

function useHomePage() {
  const t = useTranslations("home");

  return { t };
}
