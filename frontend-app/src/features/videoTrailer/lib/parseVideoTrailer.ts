import type { MovieVideo } from "@/entities/movie/types";

export function parseVideoTrailer({
  videos,
}: {
  videos: { results: MovieVideo[] };
}): string {
  const videoTrailer = videos.results.find((video) => video.type == "Trailer");
  return videoTrailer
    ? `https://www.youtube.com/watch?v=${videoTrailer.key}`
    : "";
}
