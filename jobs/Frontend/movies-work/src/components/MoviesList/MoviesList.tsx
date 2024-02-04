import { useContext } from "react";
import { AppContext } from "../../contexts/AppContext";
import MovieItem from "./MovieItem";
import styles from "./MoviesList.module.css";

export default function MoviesList() {
  const { fetchedMovies } = useContext(AppContext);

  return (
    <ul
      role="list"
      className={styles.moviesList + " flex-grow-1 px-4 divide-y"}
    >
      {fetchedMovies.map((movie) => (
        <li key={movie.id} className="">
          <MovieItem movie={movie} />
        </li>
      ))}
    </ul>
  );
}
