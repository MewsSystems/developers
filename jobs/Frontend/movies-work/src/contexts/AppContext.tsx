import { createContext } from "react";
import { useState } from "react";

export const AppContext = createContext({
  searchMovieKeyword: "",
  fetchedMovies: [],
  page: 1,
  maximumPage: null,
  changePage: (page: number) => {},
  searchMovies: (searchInputKeyword: string, page: number) => {},
});

export default function AppContextProvider({ children }) {
  const [searchMovieKeyword, setSearchMovieKeyword] = useState("");
  const [page, setPage] = useState(1);
  const [maximumPage, setMaximumPage] = useState(null);
  const [fetchedMovies, setFetchedMovies] = useState([] as any);

  const searchMovies = async (searchInputKeyword: string, page = 1) => {
    const options = {
      method: "GET",
      headers: {
        accept: "application/json",
      },
    };
    const searchKeywordURLEncoded = encodeURIComponent(searchInputKeyword);
    const response = await fetch(
      `https://api.themoviedb.org/3/search/movie?query=${searchKeywordURLEncoded}&include_adult=false&language=en-US&page=${page}&api_key=03b8572954325680265531140190fd2a`,
      options
    );
    const data = await response.json();
    // saving for list view
    setFetchedMovies(data.results);
    // saving for going back from detail page
    setSearchMovieKeyword(searchInputKeyword);
    // saving for pagination
    setMaximumPage(data.total_pages);
    console.log(data);
  };

  const changePage = (newPage: number) => {
    setPage(newPage);
  };

  const context = {
    searchMovieKeyword: searchMovieKeyword,
    fetchedMovies: fetchedMovies,
    page: page,
    maximumPage: maximumPage,
    changePage: changePage,
    searchMovies: searchMovies,
  };

  return <AppContext.Provider value={context}>{children}</AppContext.Provider>;
}
