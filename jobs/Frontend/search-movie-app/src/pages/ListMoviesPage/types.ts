export type CardMovieProps = {
  data: {
    imageURL: string;
    title: string;
    voteAverage: number;
    releaseDate: string;
    isAdult: boolean;
    voteTotalCount: number;
  };
  handleOnClick: () => void;
};
