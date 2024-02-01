import { createContext, useEffect } from "react";
import { useState } from "react";

export const AppContext = createContext({
  searchMovieKeyword: "",
  fetchedMovies: [],
  page: 1,
  maximumPage: null,
  changePage: () => {},
});

export default function AppContextProvider({ children }) {
  const [searchMovieKeyword, setSearchMovieKeyword] = useState("");
  const [page, setPage] = useState(1);

  useEffect(() => {
    async function fetchMovies(searchKeyword: string, page: number) {
      const options = {
        method: "GET",
        headers: {
          accept: "application/json",
        },
      };
      const searchKeywordURLEncoded = encodeURIComponent(searchKeyword);
      const response = await fetch(
        `https://api.themoviedb.org/3/search/movie?query=${searchKeywordURLEncoded}&include_adult=false&language=en-US&page=${page}&api_key=03b8572954325680265531140190fd2a`,
        options
      );
      const data = await response.json();
      console.log(data);
    }

    fetchMovies(searchMovieKeyword, page);
  }, [searchMovieKeyword, page]);

  const changeSearchKeyword = (newKeyword: string) => {
    setSearchMovieKeyword(newKeyword);
  };
  const changePage = (newPage: number) => {
    setPage(newPage);
  };

  const context = {
    searchMovieKeyword: searchMovieKeyword,
    changeSearchKeyword: changeSearchKeyword,
    page: page,
    changePage: changePage,
  };

  return <AppContext.Provider value={context}>{children}</AppContext.Provider>;
}
