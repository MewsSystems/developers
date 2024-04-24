import { useEffect, useRef, useState } from 'react';
import { Movie } from '../../data/interfaces';
import SearchForm from '../../components/searchForm/searchForm';
import MovieList from '../../components/movieList/movieList';
import MovieDetail from '../../components/movieDetail/movieDetail';
import PagesNavigation from '../../components/pagesNavigation/pagesNavigation';
import { getMoviesWithActualParametres } from '../../data/getMovies';

export default function HomePage() {
  const [search, setSearch] = useState<string>('');
  const [totalPagesNumber, setTotalPagesNumber] = useState<number>(1);
  const [movies, setMovies] = useState<Movie[]>([]);
  const [selectedMovieId, setSelectedMovieId] = useState<number>(-1);
  const [currentPageNumber, setCurrentPageNumber] = useState<number>(1);

  const labelRef = useRef<HTMLLabelElement>(null);

  useEffect(() => {
    getMoviesWithActualParametres(
      search,
      setMovies,
      currentPageNumber,
      setTotalPagesNumber,
    );
  }, [search, currentPageNumber]);

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  };

  const handleSelectedMovie = (movieId: number) => {
    setSelectedMovieId(movieId);
  };

  const handlePageNumberPlus = () => {
    setCurrentPageNumber((prevNumber) => prevNumber + 1);
    labelRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const handlePageNumberMinus = () => {
    setCurrentPageNumber((prevNumber) => prevNumber - 1);
    labelRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  return (
    <main>
      {selectedMovieId === -1 ? (
        <>
          <h1>Find your movie</h1>
          <SearchForm
            searchValue={search}
            searchFunction={handleSearch}
            ref={labelRef}
          />
          <MovieList
            movieValues={movies}
            handleSelectedMovie={handleSelectedMovie}
          />
          {totalPagesNumber > 1 && (
            <PagesNavigation
              pageNumber={currentPageNumber}
              totalPagesNumber={totalPagesNumber}
              increasePage={handlePageNumberPlus}
              decreasePage={handlePageNumberMinus}
            />
          )}
        </>
      ) : (
        <MovieDetail
          selectedMovieId={selectedMovieId}
          setSelectedMovie={setSelectedMovieId}
        />
      )}
    </main>
  );
}
