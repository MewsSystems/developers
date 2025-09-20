import type { Configuration } from "@/entities/configuration/types";
import type {
  Images,
  MovieDetailsAppended,
  MovieProductionCompanyWithImg,
} from "@/entities/movie/types";

export type CollectionDetail = {
  bgImage: string;
  parts: string;
  name: string;
};

export type CastImg = {
  id: number;
  width: string;
  img: string;
  name: string;
  character: string;
};
export type DetailsProps = {
  movie: MovieDetailsAppended;
  collection?: CollectionDetail;
  configuration: Configuration;
  images?: Images;
  language: string;
  productionCompaniesWithImg: MovieProductionCompanyWithImg[];
  backdrop_img: string;
  backdrop_img_url_css: string;
  poster_img: string;
  countriesOrigin: string;
  genres: string;
  duration: string;
  releaseDateLocale: string;
  releaseDateYearLocale: string;
  videoYoutubeTrailer: string;
  castImgs: CastImg[];
  media: MovieMedia;
  reviews: ReviewDetails[];
};

export type ReviewDetails = {
  id: string;
  sliced_content: string;
  fullContent: string;
};

type PosterMedia = {
  file_path: string;
  width: string;
  imgSrc: string;
};
type VideoMedia = {
  id: string;
  width: string;
  height: string;
  youtubeUrl: string;
  youtubeImgSrc: string;
};

type BackdropMedia = {
  width: string;
  file_path: string;
  imgSrc: string;
};

export type MovieMedia = {
  totalPosters: number;
  postersMedia: PosterMedia[];
  totalVideos: number;
  videosMedia: VideoMedia[];
  totalBackdrops: number;
  backdropsMedia: BackdropMedia[];
};
