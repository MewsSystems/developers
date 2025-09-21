import {
  createYoutubeImgUrl,
  parseBackdropToImgPath,
  parsePosterToImgPath,
  YOUTUBESIZES,
} from "@/shared/lib/utils";
import { slice } from "lodash";
import { parseWidth } from "@/shared/lib/utils";
import type { MovieDetailsAppended } from "@/entities/movie/types";
import type { Images } from "@/entities/movie/types";
import type { Configuration } from "@/entities/configuration/types";
import type { MovieMedia } from "@/features/media/types";

export function parseMediaAttrs({
  movie,
  images,
  configuration,
}: {
  movie: MovieDetailsAppended;
  images?: Images;
  configuration: Configuration;
}): MovieMedia {
  const MAX_SHOW = 2;
  const VIDEO_TYPE = "YouTube";
  const videos = movie.videos.results.filter((v) => v.site === VIDEO_TYPE);
  const shownVideos = slice(videos, 0, MAX_SHOW);
  const backdrops = images?.backdrops ?? [];
  const shownbackdrops = slice(backdrops, 0, MAX_SHOW);
  const backdropSizePosition = 1;
  const backdropWidth = "480px";

  const posters = images?.posters ?? [];
  const shownposters = slice(posters, 0, MAX_SHOW);
  const posterSizePosition = 2;
  const posterWidth = parseWidth(
    configuration.images.poster_sizes[posterSizePosition]
  );

  return {
    totalPosters: posters.length,
    postersMedia: shownposters.map((poster) => {
      return {
        file_path: poster.file_path,
        width: posterWidth,
        imgSrc: parsePosterToImgPath(
          configuration.images,
          poster.file_path,
          posterSizePosition
        ),
      };
    }),
    totalVideos: videos.length,
    videosMedia: shownVideos.map((video) => {
      return {
        id: video.id,
        width: "480px",
        height: "400px",
        youtubeUrl: `https://www.youtube.com/watch?v=${video.key}`,
        youtubeImgSrc: createYoutubeImgUrl(video.key, YOUTUBESIZES.HQ),
      };
    }),
    totalBackdrops: backdrops.length,
    backdropsMedia: shownbackdrops.map((backdrop) => {
      return {
        width: backdropWidth,
        file_path: backdrop.file_path,
        imgSrc: parseBackdropToImgPath(
          configuration.images,
          backdrop.file_path,
          backdropSizePosition
        ),
      };
    }),
  };
}
