import InputSearch from "../components/InputSearch";
import MoviesList from "../components/MoviesList";
import Pagination from "../components/Pagination";

export default function SearchPage() {
  return (
    <div className="flex flex-col max-h-screen ">
      <InputSearch />
      <MoviesList />
      <Pagination />
    </div>
  );
}
