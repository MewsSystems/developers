import { tmdbClient } from "@/tmdbClient";
import { useEffect, useState } from "react";
import { Flatrate, WatchLocale } from "tmdb-ts";
import styled from "styled-components";
import { Chip } from ".";
import { MEDIA_ORIGINAL_BASE_URL } from "@/tmdbClient";

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
      {streamingsForLocale?.map(({ provider_name, logo_path, provider_id }) => (
        <Chip
          key={provider_id}
          label={provider_name}
          imagePath={MEDIA_ORIGINAL_BASE_URL + logo_path}
          TypographyProps={{ color: "white" }}
        />
      ))}
    </ChipsContainer>
  );
}
