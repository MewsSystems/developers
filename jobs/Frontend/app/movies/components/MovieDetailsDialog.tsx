import {
  CloseButton,
  Dialog,
  DialogPanel,
  DialogTitle,
} from '@headlessui/react';
import { useSuspenseQuery } from '@tanstack/react-query';
import { getRouteApi } from '@tanstack/react-router';
import React from 'react';
import { AiFillCloseCircle } from 'react-icons/ai';

const routeApi = getRouteApi('/movies');

export const MovieDetailsDialog = () => {
  const movieId = routeApi.useSearch({ select: (search) => search.movieId });
  const navigate = routeApi.useNavigate();

  return (
    <React.Suspense>
      <Dialog
        open={movieId != null}
        onClose={() =>
          navigate({
            to: '.',
            search: (prev) => ({ ...prev, movieId: undefined }),
          })
        }
        className="fixed bottom-0 left-0 right-0 top-0 bg-gray-400 bg-opacity-50 dark:bg-gray-800 dark:bg-opacity-50"
      >
        {movieId != null ? <MovieDetailsDialogPanel movieId={movieId} /> : null}
      </Dialog>
    </React.Suspense>
  );
};

export interface MovieDetailsDialogPanel {
  readonly movieId: number;
}

export const MovieDetailsDialogPanel = (props: MovieDetailsDialogPanel) => {
  const { movieDetailQueryOptions } = routeApi.useRouteContext();
  const { data } = useSuspenseQuery(movieDetailQueryOptions(props.movieId));

  return (
    <div className="fixed flex h-screen w-screen items-center justify-center">
      <DialogPanel
        transition
        className="flex h-full w-full flex-col gap-5 overflow-auto rounded-lg border bg-white p-8 shadow-2xl duration-300 ease-in-out data-[closed]:opacity-0 lg:h-max xl:w-3/4 dark:border-gray-800 dark:bg-slate-800"
      >
        <div className="flex items-center gap-5">
          <CloseButton className="group">
            <AiFillCloseCircle
              className="fill-gray-500 transition-colors group-hover:fill-gray-800 dark:group-hover:fill-gray-300"
              size={30}
            />
          </CloseButton>
          <DialogTitle className="text-3xl">
            {data.original_title}
            {data.original_title !== data.title ? ` (${data.title})` : ''}
          </DialogTitle>
        </div>
        <div className="flex items-center gap-5">
          {data.poster_path != null ? (
            <div className="flex-shrink-0">
              <img
                className="rounded shadow-lg"
                src={`https://image.tmdb.org/t/p/w200/${data.poster_path}`}
              ></img>
            </div>
          ) : null}
          <div className="flex flex-col gap-5">
            <div className="text-xl">{data.runtime} min.</div>
            <div className="flex flex-wrap gap-5">
              {data.genres.map((genre, i) => (
                <div
                  key={genre.id}
                  className={`rounded-lg bg-gray-500 px-4 py-1 text-xl text-white shadow-lg dark:bg-gray-700`}
                >
                  {genre.name}
                </div>
              ))}
            </div>
            <p className="text-xl">{data.tagline}</p>
            <p className="text-xl">{data.overview}</p>
            <div className="flex gap-4">
              <p className="text-xl">{data.status}</p>
              <p className="text-xl">{data.release_date}</p>
            </div>
          </div>
        </div>
      </DialogPanel>
    </div>
  );
};
