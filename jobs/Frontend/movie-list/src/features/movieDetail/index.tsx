import { NextPage } from "next";

import { MovieDetail } from "./components/MovieDetail";

type Props = {
  id: number;
};

export const MovieDetailPage: NextPage<Props> = ({ id }) => {
  return (
    <>
      <MovieDetail id={id} />
    </>
  );
};

MovieDetailPage.getInitialProps = async (ctx) => {
  const { res, query } = ctx;
  const { id } = query;

  const convertedId = Number(id);

  if (isNaN(convertedId)) {
    if (res) {
      res.statusCode = 400;
      res.end("Invalid movie id");
    }
  }

  return { id: convertedId };
};
