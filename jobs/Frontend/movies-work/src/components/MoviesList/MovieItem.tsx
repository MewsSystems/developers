import { Link } from "react-router-dom";
import Constants from "../../config/constants";

export default function MovieItem({ movie }) {
  const isPosterAvailable = movie.poster_path !== null;
  const imagePath = `${Constants.IMAGE_URL}/w200${movie.poster_path}`;

  const time = <time dateTime={movie.release_date}>{movie.release_date}</time>;
  const isReleased: boolean = movie.release_date ? true : false;

  return (
    <Link
      to={`/movie/${movie.id}`}
      // to preserve search query params
      state={{ query: location.search }}
      className="flex justify-between py-5 gap-x-6"
    >
      <div className="flex items-center min-w-0 gap-x-4">
        <img
          className="h-12 w-12 flex-none rounded-full bg-gray-50"
          src={isPosterAvailable ? imagePath : null}
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
        <p className="text-sm leading-6 text-gray-300">⭐ {movie.popularity}</p>

        <p className="mt-1 text-xs leading-5 text-gray-500">
          {!isReleased && <span>Not released yet</span>}
          {isReleased && <span>Released&nbsp;{time}</span>}
        </p>
      </div>
    </Link>
  );
}
