export type MovieSummary = {
  id: number;
  title: string;
  overview: string;
  posterImage: string;
};

export type Movie = MovieSummary & {
  homepage: string;
  genres: string[];
  spokenLanguages: string[];
};

export type MovieSearchResult = {
  page: number;
  totalPage: number;
  movies: MovieSummary[];
}

import { IncomingMessage, ServerResponse } from "http";
import { NextApiRequestCookies } from "next/dist/server/api-utils";

export type TestReq = IncomingMessage & {
  cookies: NextApiRequestCookies;
} & jest.Mock<any>;

export type TestRes = ServerResponse & jest.Mock<any>;