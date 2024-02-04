import InputSearch from "../components/InputSearch";
import MoviesList from "../components/MoviesList/MoviesList";
import MoviesSkeletonList from "../components/MoviesList/MoviesSkeletonList";
import Pagination from "../components/Pagination/Pagination";
import { AppContext } from "../contexts/AppContext";
import { useContext } from "react";

export default function SearchPage() {
  const { isFetching, fetchedMovies } = useContext(AppContext);
  let content;

  const moviesDownloaded = fetchedMovies.length > 0;
  {
    if (!isFetching && !moviesDownloaded) {
      content = <p></p>;
    }
    if (isFetching && !moviesDownloaded) {
      content = (
        <>
          <MoviesSkeletonList />
        </>
      );
    }
    if (isFetching && moviesDownloaded) {
      content = (
        <>
          <MoviesSkeletonList />
          <Pagination />
        </>
      );
    }
    if (!isFetching && moviesDownloaded) {
      content = (
        <>
          <MoviesList />
          <Pagination />
        </>
      );
    }

    return (
      <div className="flex flex-col max-h-screen gap-3">
        <InputSearch />
        {content}
      </div>
    );
  }
}
