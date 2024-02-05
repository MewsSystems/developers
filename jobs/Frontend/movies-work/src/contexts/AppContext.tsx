import React from "react";
import { createContext } from "react";
import { useState, useEffect } from "react";
import Constants from "../config/constants";
import { IMovie, IMovieResponse } from "../types/movieTypes";
import { IChildren } from "../types/appTypes";

interface IContext {
  searchMovieKeyword: string;
  fetchedMovies: Array<IMovie>;
  isFetching: boolean;
  page: number;
  maximumPage: number;
  changeKeyword: (keyword: string) => void;
  changePage: (page: number) => void;
}

export const AppContext = createContext<IContext>({
  searchMovieKeyword: "",
  fetchedMovies: [],
  isFetching: false,
  page: 1,
  maximumPage: null,
  changeKeyword: (keyword: string) => {},
  changePage: (page: number) => {},
});

const AppContextProvider = ({ children }: IChildren) => {
  const [searchMovieKeyword, setSearchMovieKeyword] = useState("");
  const [page, setPage] = useState(1);
  const [maximumPage, setMaximumPage] = useState(null);
  const [fetchedMovies, setFetchedMovies] = useState<IMovie[]>([]);
  const [isFetching, setIsFetching] = useState(false);

  const searchMovies = async (searchInputKeyword: string, page = 1) => {
    const options = {
      method: "GET",
      headers: {
        accept: "application/json",
      },
    };
    // TODO optimize the URL creation - use new URL(...)
    const searchKeywordURLEncoded = encodeURIComponent(searchInputKeyword);
    setIsFetching(true);
    const response = await fetch(
      `${Constants.API_URL}/${Constants.API_VERSION}/search/movie?query=${searchKeywordURLEncoded}&include_adult=false&language=en-US&page=${page}&api_key=${Constants.API_KEY}`,
      options
    );
    const data: IMovieResponse = await response.json();
    setIsFetching(false);
    // saving for list view
    const movies: IMovie[] = data.results;
    setFetchedMovies(movies);
    // saving for pagination
    setMaximumPage(data.total_pages);
    console.log(data);
  };

  useEffect(() => {
    if (searchMovieKeyword) {
      searchMovies(searchMovieKeyword, page);
    }
  }, [searchMovieKeyword, page]);

  const changeKeyword = (newKeyword: string) => {
    setPage(1);
    setSearchMovieKeyword(newKeyword);
  };
  const changePage = (newPage: number) => {
    setPage(newPage);
  };

  const context = {
    searchMovieKeyword: searchMovieKeyword,
    fetchedMovies: fetchedMovies,
    isFetching: isFetching,
    page: page,
    maximumPage: maximumPage,
    changeKeyword: changeKeyword,
    changePage: changePage,
  };

  return <AppContext.Provider value={context}>{children}</AppContext.Provider>;
};

export default AppContextProvider;
