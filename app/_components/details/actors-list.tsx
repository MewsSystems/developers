import { apiConfig } from "@/domain/remote/config";
import { Group } from "@/styles/base/group";
import { Stack } from "@/styles/base/stack";
import { Text } from "@/styles/base/text";
import { Title } from "@/styles/base/title";
import { ActorItem } from "@/styles/components/actors";
import { Actor } from "@/types/actor";
import { useTranslations } from "next-intl";
import { FC } from "react";

type Props = {
  actors: Actor[];
};

export const ActorsList: FC<Props> = ({ actors }) => {
  const t = useTranslations("movie");

  const renderActor = (actor: Actor) => (
    <Stack $gap="xs" $align="center">
      <ActorItem
        key={actor.id}
        $bgImage={apiConfig.posterImage(actor.profile_path)}
      />
      <Text $size="sm" $ta="center">
        {actor.name}
      </Text>
    </Stack>
  );

  return (
    <Stack $gap="md">
      <Title $variant="h3">{t("actors")}</Title>
      <Group $gap="lg">{actors.map(renderActor)}</Group>
    </Stack>
  );
};
