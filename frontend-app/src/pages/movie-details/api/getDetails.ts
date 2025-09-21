import { getConfiguration } from "@/entities/configuration/api/configurationApi";
import { getMovie } from "@/entities/movie/api/getMovieApi";
import { getMovieCollection } from "@/entities/movie/api/getMovieCollectionApi";
import { getMovieImages } from "@/entities/movie/api/getMovieImagesApi";
import {
  parseBackdropToImgPath,
  parsePosterToImgPath,
} from "@/shared/lib/utils";
import type { DetailsProps } from "@/pages/movie-details/types";
import { parseCrewDirectors } from "@/features/crewDirectors/lib/parseCrewDirectors";
import { parseMediaAttrs } from "@/features/media/lib/parseMedia";
import { parseReviews } from "@/features/social/lib/parseReview";
import { parseCastImgs } from "@/features/topBilledCast/lib/parseCast";
import { parseVideoTrailer } from "@/features/videoTrailer/lib/parseVideoTrailer";
import { parseCollection } from "@/features/collection/lib/parseCollection";
import { parseRecommendations } from "@/features/recommendation/lib/parseRecommendations";
import { parseInfo } from "@/features/info/lib/parseInfo";
import { parseTitle } from "@/features/title/lib/parseTitle";

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

  return {
    blocked: movie.adult,
    title: parseTitle({ movie, language }),
    info: parseInfo({ movie }),
    favorite: movie.account_states.favorite,
    watchlist: movie.account_states.watchlist,
    movieId: movie.id,
    tagline: movie.tagline,
    overview: movie.overview,
    voteAverage: Math.round(movie.vote_average * 10),
    images,
    configuration,
    language,
    backdrop_img_url_css,
    poster_img,
    videoYoutubeTrailer: parseVideoTrailer({ videos: movie.videos }),
    castImgs: parseCastImgs({ movie, configuration }),
    media: parseMediaAttrs({ movie, images, configuration }),
    totalReviews: movie.reviews.results.length,
    reviews: parseReviews({ movie }),
    collection: collection
      ? parseCollection({ collection, configuration, language })
      : undefined,
    crewDirectors: parseCrewDirectors({
      crew: movie.credits?.crew ?? [],
      language,
    }),
    recommendations: parseRecommendations({
      recommendations: movie.recommendations.results,
      configuration,
    }),
  };
}
