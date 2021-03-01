import { useAppSelector } from '../../hooks';
import Container from '../common/Container';
import MovieCard, { CardsList } from '../MovieCard';
import Pagination from '../Pagination';
import EmptyState from '../EmptyState';
import { Link } from 'react-router-dom';
import Button from '../common/Button';
import ErrorState from '../EmptyState/ErrorState';
import { Binoculars } from '@styled-icons/fa-solid';

interface SearchResultsProps {
  onPageClick: (page: number) => void;
}

const SearchResults = ({ onPageClick }: SearchResultsProps) => {
  const {
    isLoading,
    error,
    page,
    total_pages,
    results,
    query,
  } = useAppSelector((state) => state.search);

  if (isLoading) {
    return <EmptyState title="Searching..." />;
  }

  if (error) {
    return (
      <ErrorState title={error?.name || 'Unknown Error'}>
        <div>
          {error?.message}
          <Link to="/" component={Button}>
            Refresh
          </Link>
        </div>
      </ErrorState>
    );
  }

  if (query && !results.length) {
    return (
      <EmptyState title="No Movies Found" icon={<Binoculars size="5rem" />}>
        Couldn't find movies for the query: <em>{query}</em>
      </EmptyState>
    );
  }

  return (
    <Container>
      <CardsList>
        {results.map((movie) => (
          <MovieCard
            key={movie.id}
            {...movie}
            height="140px"
            isLink
            to={`/movie/${movie.id}`}
          />
        ))}
      </CardsList>
      <Pagination
        page={page}
        pageCount={total_pages}
        onPageClick={onPageClick}
      />
    </Container>
  );
};

export default SearchResults;
