import React from "react";
import { IMovie } from "./movieTypes";

export interface IChildren {
  children: React.ReactNode;
}
export interface IContext {
  searchMovieKeyword: string;
  fetchedMovies: IMovie[];
  isFetching: boolean;
  page: number;
  maximumPage: number | null;
  setAppSearchParams: (keyword?: string, page?: number) => void;
}
