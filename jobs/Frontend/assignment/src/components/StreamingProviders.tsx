import { tmdbClient } from "@/pages/Search";
import { useEffect, useState } from "react";
import { Flatrate, WatchLocale } from "tmdb-ts";
import styled from "styled-components";
import { Chip } from ".";

// TODO: move this to sharing types
export type WithMovieIdProps<T = unknown> = T & { movieId: number };

type LocalesWithoutStreaming = "AT" | "DE";
type WatchLocalesWithFlatrate = Omit<WatchLocale, LocalesWithoutStreaming>;

const ChipsContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
`;

export function StreamingProviders({ movieId }: WithMovieIdProps) {
  const [streamingsForLocale, setStreamingsForLocale] = useState<Flatrate[]>();

  const userLang = navigator.language.split("-")[1];

  useEffect(() => {
    tmdbClient.movies.watchProviders(movieId).then(res => {
      const doesNotHaveFlatrate = !("flatrate" in res.results[userLang as keyof WatchLocale]);
      if (doesNotHaveFlatrate) return;

      const result = res.results as WatchLocalesWithFlatrate;
      setStreamingsForLocale(result[userLang as keyof WatchLocalesWithFlatrate].flatrate);
    });
  }, []);

  return (
    <ChipsContainer>
      {streamingsForLocale?.map(({ provider_name, logo_path }) => (
        <Chip label={provider_name} imagePath={"https://image.tmdb.org/t/p/original" + logo_path} />
      ))}
    </ChipsContainer>
  );
}
