export type RawMovieDetailsType = {
  id: string;
  overview: string | null;
  title: string;
  poster_path: string | null;
  release_date: string;
  budget: string | null;
  original_title: string;
  vote_average: number | null;
  vote_count: number | null;
};

export type MovieDetailsType = {
  id: string;
  overview: string;
  title: string;
  imgUrl: string;
  releaseDate: string;
  budget: string | null;
  originalTitle: string;
  rating: number;
  voteCount: string | null;
};

export type MovieDetailsStateType = {
  movie: RawMovieDetailsType | null;
  isLoading: boolean;
  errorMessage?: string | null;
};
