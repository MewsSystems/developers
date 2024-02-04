import InputSearch from "../components/InputSearch";
import MoviesList from "../components/MoviesList";
import Pagination from "../components/Pagination";
import styles from "./SearchPage.module.css";

export default function SearchPage() {
  return (
    <div className="flex flex-col max-h-screen ">
      <div className="">
        <InputSearch />
      </div>
      <div className={styles.moviesList + " flex-grow-1 px-4 mt-2"}>
        <MoviesList />
      </div>
      <div className="">
        <Pagination />
      </div>
    </div>
  );
}
