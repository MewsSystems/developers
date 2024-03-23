import { debounce } from 'lodash';
import { useState } from 'react';
import { Link } from 'react-router-dom';

import css from './home.module.css';

import { MovieCard } from '@/components/movieCard/MovieCard';
import { Page } from '@/components/page/Page';
import { Pagination } from '@/components/pagination/Pagination';
import { useMoviesSearchQuery } from '@/queries/moviesQueries';

export const Home = () => {
  const [search, setSearch] = useState('');
  const [currentPage, setCurrentPage] = useState(1);

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
    setCurrentPage(1);
  };

  const debouncedSearch = debounce(handleSearch, 400);

  const {
    data,
    isLoading: isMovieListLoading,
    error,
  } = useMoviesSearchQuery(search, currentPage);

  const isSearchEmpty = !search;
  const isError = search && error;
  const isLoading = search && isMovieListLoading;
  const isFoundData = search && data && data.results.length > 0;
  const isNoResults = search && data && data.results.length === 0;

  return (
    <Page>
      <div className={css.contentContainer}>
        <h1 className={css.title}>Mews Movie Search</h1>
        <input onChange={debouncedSearch} />
        <hr className={css.divider} />

        {isSearchEmpty && (
          <h1 className={css.title}>Please type something to begin search</h1>
        )}

        {isLoading && <h1 className={css.title}>Loading...</h1>}

        {isError && (
          <h1 className={css.title}>
            There seems to have been an error, the API might be down. Refer to
            the console for more details.
          </h1>
        )}

        {isFoundData && (
          <>
            <Pagination
              numberOfPages={data.total_pages}
              currentPage={currentPage}
              setCurrentPage={setCurrentPage}
            />

            <div className={css.movieCardsContainer}>
              {!isMovieListLoading && data && (
                <>
                  {data.results.map((movie) => (
                    <Link key={movie.id} to={`/${movie.id}`} state={movie}>
                      <MovieCard movie={movie} />
                    </Link>
                  ))}
                </>
              )}
            </div>
          </>
        )}

        {isNoResults && (
          <h1 className={css.title}>
            No results found, please alter your search
          </h1>
        )}
      </div>
    </Page>
  );
};
