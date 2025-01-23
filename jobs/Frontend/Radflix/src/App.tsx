import { useState } from "react";
import "./App.css";
import "./CSS/Scroll.css";

import Search from "./components/Search.tsx";
import MovieList from "./components/MovieList.tsx";
import PageNav from "./components/PageNav.tsx";
import Skeleton from "./components/Skeleton.tsx";

function App() {
  const [searchResults, setSearchResults] = useState<[]>();
  const [pageAmount, setPageAmount] = useState<number>();
  const [page, setPage] = useState<number>(1);
  const [loading, setLoading] = useState(false);
  const [scoreFilter, setScoreFilter] = useState("");

  const handleSearchResults = (results: object) => {
    setSearchResults(Object.entries(results)[1][1]);
    setPageAmount(Object.entries(results)[2][1]);
  };

  const handleScore = (score: string) => {
    setScoreFilter(score);
  };

  const handlePageTurn = (newPage: number) => {
    setPage(newPage);
  };

  const handleLoading = (loading: boolean) => {
    setLoading(loading);
    setTimeout(() => {
      setLoading(!loading);
    }, 1000);
  };

  return (
    <>
      <div
        className={
          !searchResults && !loading
            ? "search-container-home"
            : "search-container-bar"
        }
      >
        <h1
          className="app-title"
          onClick={() =>
            handleSearchResults(
              {
                page: 0,
                results: null,
                total_pages: undefined,
              }
            )
          }
        >
          <b>RAD</b>FLIX
        </h1>
        <Search
          resultsHandler={handleSearchResults}
          page={page}
          resetPage={handlePageTurn}
          showLoading={handleLoading}
          updateScore={handleScore}
        />
      </div>

      {searchResults ? (
        <MovieList
          results={searchResults}
          score={scoreFilter}
          visible={!loading}
        />
      ) : null}
      <Skeleton visible={loading} />

      <PageNav
        pageAmount={pageAmount}
        page={page}
        changePage={handlePageTurn}
      />
    </>
  );
}

export default App;
