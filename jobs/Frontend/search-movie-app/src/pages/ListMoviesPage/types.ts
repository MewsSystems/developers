export type CardMovieProps = {
  data: {
    imageURL: string | null;
    title: string;
    voteAverage: number;
    releaseDate: string | null;
    isAdult: boolean;
    voteTotalCount: number;
  };
  handleOnClick: () => void;
};
