import { useContext } from "react";
import { AppContext } from "../../contexts/AppContext";
import { Link } from "react-router-dom";
import styles from "./MoviesList.module.css";

export default function MoviesList() {
  const { fetchedMovies } = useContext(AppContext);

  return (
    <ul
      role="list"
      className={
        styles.moviesList + " flex-grow-1 px-4 mt-2 divide-y divide-gray-100"
      }
    >
      {fetchedMovies.map((movie) => (
        <li key={movie.id} className="">
          <Link
            to={`/movie/${movie.id}`}
            className="flex justify-between py-5 gap-x-6"
          >
            <div className="flex items-center min-w-0 gap-x-4">
              {/* TODO loading image */}
              <img
                className="h-12 w-12 flex-none rounded-full bg-gray-50"
                src={movie.imageUrl}
                alt=""
              />
              <div className="min-w-0 flex-auto text-left">
                <p className="text-sm font-semibold leading-6 text-gray-300">
                  {movie.title}
                </p>
                {/* TODO overview should end with ellipsis not with cliping text ... */}
                <p className="after:content-['...'] mt-1 text-ellipsis text-wrap max-h-10 overflow-y-hidden text-xs leading-5 text-gray-500">
                  {movie.overview}
                </p>
              </div>
            </div>
            <div className="hidden shrink-0 sm:flex sm:flex-col sm:items-end">
              <p className="text-sm leading-6 text-gray-300">
                ⭐ {movie.popularity}
              </p>
              {/* TODO release date could be absent */}
              <p className="mt-1 text-xs leading-5 text-gray-500">
                Released{" "}
                <time dateTime={movie.release_date}>{movie.release_date}</time>
              </p>
            </div>
          </Link>
        </li>
      ))}
    </ul>
  );
}
