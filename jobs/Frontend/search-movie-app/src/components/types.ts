type ImageParams = {
  imageURL: string | null;
  title: string;
  fileSize: string;
  rounded?: boolean;
  mobileImageURL?: string | null;
};

type VoteParams = {
  voteAverage: number;
  voteTotalCount: number;
};

export type { VoteParams, ImageParams };
