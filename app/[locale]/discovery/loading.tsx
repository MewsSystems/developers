import { Box } from "@/styles/base/box";
import { Stack } from "@/styles/base/stack";
import { Title } from "@/styles/base/title";
import { LoadingContainer } from "@/styles/components/loading-container";
import { getTranslations } from "next-intl/server";

export default async function Loading() {
  const { title } = await useLoading();

  return (
    <LoadingContainer $justify="center" $align="center">
      <Title>{title}</Title>
    </LoadingContainer>
  );
}

async function useLoading() {
  const t = await getTranslations("shared");
  const title = t("loading");

  return { title };
}
