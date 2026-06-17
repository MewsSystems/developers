import { tmdbClient } from "@/tmdbClient";
import { useEffect, useMemo, useState } from "react";
import { Genre } from "tmdb-ts";

export function useGenres() {
  const [genres, setGenres] = useState<Genre[]>([]);

  useEffect(() => {
    tmdbClient.genres.movies().then(({ genres }) => setGenres(genres));
  }, []);

  const genresMap = useMemo(() => {
    return new Map(genres.map(genre => [genre.id, genre.name]));
  }, [genres]);

  const getGenreNameById = (id: number) => {
    return genresMap.get(id) || "Unknown";
  };

  return { genres, genresMap, getGenreNameById };
}
