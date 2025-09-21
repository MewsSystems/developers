export type MovieMedia = {
  totalPosters: number;
  postersMedia: PosterMedia[];
  totalVideos: number;
  videosMedia: VideoMedia[];
  totalBackdrops: number;
  backdropsMedia: BackdropMedia[];
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

