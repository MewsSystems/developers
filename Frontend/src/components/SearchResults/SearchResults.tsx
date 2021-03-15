import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { Binoculars } from '@styled-icons/fa-solid';

import Container from '../common/Container';
import MovieCard, { CardsList } from '../MovieCard';
import Pagination from '../Pagination';
import EmptyState, { ErrorState, LoadingState } from '../EmptyState';
import Button from '../common/Button';
import { searchSelector } from '../../redux/searchReducer';
import { useSearchQueryParams } from '../../hooks';

function SearchResults() {
  const search = useSelector(searchSelector);
  const [, setQueryParams] = useSearchQueryParams();
  const setPage = (page: number) => setQueryParams({ page });

  if (search.isLoading) {
    return <LoadingState title="Searching..." />;
  }

  if (search.error) {
    return (
      <ErrorState title={search.error?.name || 'Unknown Error'}>
        <div>
          {search.error?.message}
          <Link to="/" component={Button}>
            Refresh
          </Link>
        </div>
      </ErrorState>
    );
  }

  if (search.query && !search.results.length) {
    return (
      <EmptyState title="No Movies Found" icon={<Binoculars size="5rem" />}>
        Couldn't find movies for the query: <em>{search.query}</em>
      </EmptyState>
    );
  }

  if (!search.query) {
    return (
      <EmptyState title="Movie Search">
        Start typing on the search box above to find a movie.
      </EmptyState>
    );
  }

  return (
    <Container>
      <CardsList>
        {search.results.map((movie) => (
          <MovieCard
            {...movie}
            key={movie.id}
            height="140px"
            isLink
            to={`/movie/${movie.id}`}
          />
        ))}
      </CardsList>
      <Pagination
        page={search.page}
        pageCount={search.total_pages}
        onPageClick={setPage}
      />
    </Container>
  );
}

export default SearchResults;
