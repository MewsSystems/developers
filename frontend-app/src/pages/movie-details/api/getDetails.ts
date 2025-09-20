import { getConfiguration } from "@/entities/configuration/api/configurationApi";
import { getMovie } from "@/entities/movie/api/getMovieApi";
import { getMovieCollection } from "@/entities/movie/api/getMovieCollectionApi";
import { getMovieImages } from "@/entities/movie/api/getMovieImagesApi";
import {
  createYoutubeImgUrl,
  formatList,
  includeConfiguration,
  parseBackdropToImgPath,
  parsePosterToImgPath,
  toDurationFormat,
  toLocaleDate,
  toLocaleYear,
  toProductionComponiesWithImg,
  YOUTUBESIZES,
} from "@/shared/lib/utils";
import { slice } from "lodash";
import { parseProfileToImgPath, parseWidth } from "@/shared/lib/utils";
import type {
  Collection,
  MovieDetailsAppended,
  MovieVideo,
} from "@/entities/movie/types";
import type { Images } from "@/entities/movie/types";
import type { Configuration } from "@/entities/configuration/types";
import type {
  CastImg,
  CollectionDetail,
  DetailsProps,
  MovieMedia,
} from "@/pages/movie-details/types";

export async function getDetails(
  { movie_id }: { movie_id: string },
  { language, session_id }: { language: string; session_id: string }
): Promise<DetailsProps> {
  const movie = await getMovie({ movie_id }, { language, session_id });
  const images = await getMovieImages({ movie_id });
  const collection = movie?.belongs_to_collection
    ? await getMovieCollection(
        { collection_id: movie.belongs_to_collection.id },
        { language }
      )
    : undefined;
  const configuration = await getConfiguration();
  if (movie == undefined) {
    throw Error("no movie found for this movie");
  }

  if (configuration == undefined) {
    throw Error("no configuration found");
  }

  const boundIncludeConfiguration = includeConfiguration.bind(
    null,
    configuration.images
  );
  const productionCompaniesWithImg = toProductionComponiesWithImg(
    boundIncludeConfiguration,
    movie.production_companies
  );

  const backdrop_img = parseBackdropToImgPath(
    configuration.images,
    movie.backdrop_path ?? "",
    6
  );
  const backdrop_img_url_css = `url(${backdrop_img})`;
  const poster_img = parsePosterToImgPath(
    configuration.images,
    movie.poster_path,
    3
  );

  const countriesOrigin = movie.origin_country.map((c) => c).join(", ");
  const genres = movie.genres.map((g) => g.name).join(", ");
  const duration = toDurationFormat(movie.runtime);
  const releaseDateLocale = toLocaleDate(movie.release_date, language);
  const releaseDateYearLocale = toLocaleYear(movie.release_date, language);

  return {
    movie,
    images,
    productionCompaniesWithImg,
    configuration,
    language,
    backdrop_img,
    backdrop_img_url_css,
    poster_img,
    countriesOrigin,
    genres,
    duration,
    releaseDateLocale,
    releaseDateYearLocale,
    videoYoutubeTrailer: parseVideoTrailer({ videos: movie.videos }),
    castImgs: parseCastImgs({ movie, configuration }),
    media: parseMediaAttrs({ movie, images, configuration }),
    reviews: parseReviews({ movie }),
    collection: collection
      ? parseCollection({ collection, configuration, language })
      : undefined,
  };
}

function parseMediaAttrs({
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

function parseReviews({ movie }: { movie: MovieDetailsAppended }) {
  const MAX_TEXT = 100;
  const reviews = movie.reviews.results;
  const reviewsToShow = slice(reviews, 0, 1);
  return reviewsToShow.map((review) => {
    return {
      id: review.id,
      sliced_content: review.content.slice(0, MAX_TEXT),
      fullContent: review.content,
    };
  });
}

function parseCastImgs({
  movie,
  configuration,
}: {
  movie: MovieDetailsAppended;
  configuration: Configuration;
}): CastImg[] {
  const MAX_CAST = 9;
  const cast = slice(movie.credits.cast, 0, MAX_CAST);
  const profileSizePosition = 1;
  const profileWidth = parseWidth(
    configuration.images.profile_sizes[profileSizePosition]
  );

  return cast.map((c) => {
    return {
      id: c.id,
      width: profileWidth,
      img: parseProfileToImgPath(
        configuration.images,
        c.profile_path + "",
        profileSizePosition
      ),
      character: c.character,
      name: c.name,
    };
  });
}

function parseVideoTrailer({
  videos,
}: {
  videos: { results: MovieVideo[] };
}): string {
  const videoTrailer = videos.results.find((video) => video.type == "Trailer");
  return videoTrailer
    ? `https://www.youtube.com/watch?v=${videoTrailer.key}`
    : "";
}

function parseCollection({
  collection,
  configuration,
  language,
}: {
  collection: Collection;
  configuration: Configuration;
  language: string;
}): CollectionDetail {
  const backdropSizePosition = 6;

  const backdrop_img_path = collection.backdrop_path
    ? parseBackdropToImgPath(
        configuration.images,
        collection.backdrop_path,
        backdropSizePosition
      )
    : "";
  const bgImage = `url(${backdrop_img_path})`;
  const parts = formatList(
    collection.parts.map(({ title }) => title),
    language
  );

  return {
    bgImage,
    parts,
    name: collection.name,
  };
}
