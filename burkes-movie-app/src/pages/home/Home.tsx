import { useState } from 'react';

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

  const { data, isLoading: isMovieListLoading } = useMoviesSearchQuery(
    search,
    currentPage
  );

  const isSearchEmpty = !search;
  const loading = search && isMovieListLoading;
  const foundData = search && data && data.results.length > 0;
  const noResults = search && data && data.results.length === 0;

  return (
    <Page>
      <div className={css.contentContainer}>
        <h1 className={css.title}>Mews Movie Search</h1>
        <input value={search} onChange={handleSearch} />
        <hr className={css.divider} />

        {isSearchEmpty && (
          <h1 className={css.title}>Please type something to begin search</h1>
        )}

        {loading && <h1 className={css.title}>Loading...</h1>}

        {foundData && (
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
                    <MovieCard key={movie.id} movie={movie} />
                  ))}
                </>
              )}
            </div>
          </>
        )}

        {noResults && (
          <h1 className={css.title}>
            No results found, please alter your search
          </h1>
        )}
      </div>
    </Page>
  );
};
