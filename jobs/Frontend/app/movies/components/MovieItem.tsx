import { Movie } from '../server/entities';
import { Transition } from '@headlessui/react';

export interface MovieItemProps {
  readonly movie: Movie;
  readonly renderLink: (children: React.ReactNode) => React.ReactNode;
}

export const MovieItem: React.FunctionComponent<MovieItemProps> = ({
  movie,
  renderLink,
}) => {
  return (
    <Transition show appear>
      <li
        key={movie.id}
        className="relative rounded shadow transition duration-300 ease-in data-[closed]:opacity-0"
      >
        {renderLink(
          <>
            {movie.poster_path && (
              <img
                className="absolute bottom-0 left-0 right-0 h-full rounded shadow"
                src={`https://image.tmdb.org/t/p/w200/${movie.poster_path}`}
              ></img>
            )}
            {!movie.poster_path ? (
              <div className="flex h-full w-full items-center justify-center text-2xl">
                {movie.title}
              </div>
            ) : null}
            <div className="z-1 absolute bottom-0 left-0 top-0 h-full w-full bg-white opacity-0 transition-opacity hover:opacity-50 dark:bg-gray-800"></div>
          </>,
        )}
      </li>
    </Transition>
  );
};
