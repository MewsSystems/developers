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
  setAppSearchParams: (keyword?: string, page?: number) => void;
}

export const AppContext = createContext<IContext>({
  searchMovieKeyword: "",
  fetchedMovies: [],
  isFetching: false,
  page: 1,
  maximumPage: null,
  setAppSearchParams: (keyword?: string, page?: number) => {},
});

const AppContextProvider = ({ children }: IChildren) => {
  const [searchMovieKeyword, setSearchMovieKeyword] = useState("");
  const [page, setPage] = useState(1);
  const [maximumPage, setMaximumPage] = useState(null);
  const [fetchedMovies, setFetchedMovies] = useState<IMovie[]>([]);
  const [isFetching, setIsFetching] = useState(false);
  const searchMovies = async (searchInputKeyword: string, page: number = 1) => {
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
  };
  const currentURLSearchParams = new URLSearchParams(window.location.search);

  useEffect(() => {
    if (searchMovieKeyword) {
      searchMovies(searchMovieKeyword, page);
    }
  }, [searchMovieKeyword, page]);

  const setAppSearchParams = (keyword?: string, page?: number) => {
    const realKeyword =
      keyword || searchMovieKeyword || currentURLSearchParams.get("movie");
    if (!realKeyword) {
      return;
    }

    setSearchMovieKeyword(realKeyword);

    // can happen when accesing page directly for the first time in URL
    // www.mysite.com/?movie=batman&page=notanumber
    const pageParamNumber = parseInt(
      page || currentURLSearchParams.get("page") || 1
    );
    if (!isNaN(pageParamNumber)) {
      setPage(pageParamNumber);
    }

    // TODO BUG - setting the url like this wont let you go back in browser when switching between pagination pages
    const newUrl = new URL(location);
    newUrl.searchParams.set("movie", realKeyword);
    newUrl.searchParams.set("page", pageParamNumber.toString());
    history.pushState({}, "", newUrl);
  };

  useEffect(() => {
    setAppSearchParams();
  }, []);

  const context = {
    searchMovieKeyword: searchMovieKeyword,
    fetchedMovies: fetchedMovies,
    isFetching: isFetching,
    page: page,
    maximumPage: maximumPage,
    setAppSearchParams: setAppSearchParams,
  };

  return <AppContext.Provider value={context}>{children}</AppContext.Provider>;
};

export default AppContextProvider;
