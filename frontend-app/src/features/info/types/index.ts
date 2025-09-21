import type { Keyword } from "@/entities/movie/types";

export type MovieInfo = {
  imdbLink: string;
  homepage: string;
  status: string;
  originalLanguage: string;
  budget: string;
  revenue: string;
  keywords: Keyword[];
};
