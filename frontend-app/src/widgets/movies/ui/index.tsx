import { useMovies } from '../hooks/useMovies';
import { MoviesList } from './MoviesList';
import { SearchBar } from '@/features/movie-search';
import { Pagination } from '@/features/pagination';

/**
 * Виджет для отображения фильмов с поиском и пагинацией
 */
export const Movies = () => {
  const {
    movies,
    isLoading,
    error,
    currentPage,
    totalPages,
    searchQuery,
    handleSearch,
    handlePageChange,
  } = useMovies();

  return (
    <div className="space-y-6">
      <SearchBar 
        onSearch={handleSearch}
        initialQuery={searchQuery}
        isLoading={isLoading}
      />
      <MoviesList 
        movies={movies}
        isLoading={isLoading}
        error={error}
      />
      <Pagination 
        currentPage={currentPage}
        totalPages={totalPages}
        onPageChange={handlePageChange}
        isLoading={isLoading}
      />
    </div>
  );
};