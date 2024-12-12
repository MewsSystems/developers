import * as React from 'react';
import {
  createFileRoute,
  Link,
  Outlet,
  useMatch,
  useMatches,
} from '@tanstack/react-router';
import * as v from 'valibot';
import { useDebounce } from '../../hooks/useDebounce';

import { AiOutlineSearch, AiOutlineClose } from 'react-icons/ai';
import { movieDetailQueryOptions } from '../../movies/server/detail';
import { MovieDetailsDialog } from '../../movies/components/MovieDetailsDialog';

export const Input = () => {
  const debounce = useDebounce(500);
  const query = Route.useSearch({ select: (search) => search.query });
  const [search, setSearch] = React.useState(query);
  const navigate = Route.useNavigate();

  return (
    <div className="relative w-full">
      <AiOutlineSearch
        className="absolute left-2 top-2 fill-gray-600"
        size={40}
      />
      <input
        className="w-full rounded-lg border border-gray-200 px-16 py-3 text-xl shadow focus:border-gray-400 focus:outline-none focus:ring-gray-400 dark:text-black"
        placeholder="Search for a movie"
        value={search ?? ''}
        onChange={(e) => {
          const text = !e.target.value ? undefined : e.target.value;

          setSearch(text);

          debounce(() => {
            navigate({
              to: text ? '/movies/search' : '/movies',
              search: { query: text },
            });
          });
        }}
      ></input>
      {query ? (
        <Link
          className="group absolute right-2 top-3"
          to="/movies"
          onClick={() => setSearch('')}
        >
          <AiOutlineClose
            className="fill-gray-600 transition-colors group-hover:fill-gray-800"
            size={30}
          />
        </Link>
      ) : null}
    </div>
  );
};

export const Title = () => {
  const lastMatch = useMatches({
    select: (matches) => matches[matches.length - 1],
  });
  let title = 'Movies';

  switch (lastMatch.fullPath) {
    case '/movies/':
      title = 'Popular Movies';
      break;
    case '/movies/search':
      title = `Your search${lastMatch.search.query ? ` "${lastMatch.search.query}"` : ''}`;
      break;
  }

  return <h1 className="py-3 font-serif text-4xl">{title}</h1>;
};

export const Movies = () => {
  return (
    <main className="flex justify-center py-4">
      <section className="flex flex-col gap-4">
        <Title />
        <Input />
        <div className="flex flex-col gap-3">
          <Outlet />
        </div>
        <MovieDetailsDialog />
      </section>
    </main>
  );
};

export const Route = createFileRoute('/movies')({
  validateSearch: v.object({
    query: v.optional(v.string()),
    movieId: v.optional(v.number()),
  }),
  loaderDeps: ({ search }) => ({
    movieId: search.movieId,
  }),
  context: () => ({
    movieDetailQueryOptions: movieDetailQueryOptions,
  }),
  loader: async ({ context: { queryClient }, deps: { movieId } }) => {
    if (movieId) {
      await queryClient.ensureQueryData(movieDetailQueryOptions(movieId));
    }
  },
  component: Movies,
});
