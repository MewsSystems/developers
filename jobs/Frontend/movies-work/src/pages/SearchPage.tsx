import InputSearch from "../components/InputSearch";
import MoviesList from "../components/MoviesList/MoviesList";
import Pagination from "../components/Pagination/Pagination";

export default function SearchPage() {
  return (
    <div className="flex flex-col max-h-screen ">
      <InputSearch />
      <MoviesList />
      <Pagination />
    </div>
  );
}
