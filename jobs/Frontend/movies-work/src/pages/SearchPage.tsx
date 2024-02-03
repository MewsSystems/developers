import InputSearch from "../components/InputSearch";
import MoviesList from "../components/MoviesList";
import Pagination from "../components/Pagination";

export default function SearchPage() {
  return (
    <>
      <InputSearch />
      <MoviesList />
      <Pagination />
    </>
  );
}
