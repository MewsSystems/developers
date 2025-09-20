import type { Configuration } from "@/entities/configuration/types";
import type { Images, MovieDetailsAppended } from "@/entities/movie/types";
import type { MovieMedia } from "@/features/media/types";
import type { ReviewDetails } from "@/features/social/types";
import type { CastImg } from "@/features/topBilledCast/types";
import type { CrewDirectorDetails } from "@/features/crewDirectors/types";
import type { CollectionDetail } from "@/features/collection/types";
import type { RecommendationDetail } from "@/features/recommendation/types";
import type { MovieInfo } from "@/features/info/types";
import type { MovieTitle } from "@/features/title/types";

export type DetailsProps = {
  title: MovieTitle;
  info: MovieInfo;
  movie: MovieDetailsAppended;
  collection?: CollectionDetail;
  configuration: Configuration;
  images?: Images;
  language: string;
  backdrop_img_url_css: string;
  poster_img: string;
  videoYoutubeTrailer: string;
  castImgs: CastImg[];
  media: MovieMedia;
  reviews: ReviewDetails[];
  crewDirectors: CrewDirectorDetails[];
  recommendations: RecommendationDetail[];
};
