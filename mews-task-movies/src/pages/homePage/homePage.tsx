import { SetStateAction, useEffect, useRef, useState } from 'react';
import SearchForm from '../../components/searchForm/searchForm';
import MovieList from '../../components/movieList/movieList';
import MovieDetail from '../../components/movieDetail/movieDetail';
import PagesNavigation from '../../components/pagesNavigation/pagesNavigation';
import { getMovies } from '../../data/getMovies';

export default function HomePage() {
  const [search, setSearch] = useState('');
  const [totalPagesNumber, setTotalPagesNumber] = useState(Number);
  const [movies, setMovies] = useState([]);
  const [selectedMovieId, setSelectedMovieId] = useState(Number);
  const [currentPageNumber, setCurrentPageNumber] = useState(1);

  const labelRef = useRef<HTMLLabelElement>(null);

  useEffect(() => {
    getMovies(search, setMovies, currentPageNumber, setTotalPagesNumber);
  }, [search, currentPageNumber]);

  const handleSearch = (e: { target: { value: SetStateAction<string> } }) => {
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

  console.log('movies', movies);
  console.log('selectedMovie', selectedMovieId);
  console.log('pageNumber', currentPageNumber);
  console.log('totalPagesNumber', totalPagesNumber);

  return (
    <main>
      {selectedMovieId === 0 && (
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
          {search.length > 0 && (
            <PagesNavigation
              pageNumber={currentPageNumber}
              totalPagesNumber={totalPagesNumber}
              increasePage={handlePageNumberPlus}
              decreasePage={handlePageNumberMinus}
            />
          )}
        </>
      )}
      {selectedMovieId > 0 && (
        <MovieDetail
          selectedMovieId={selectedMovieId}
          setSelectedMovie={setSelectedMovieId}
        />
      )}
    </main>
  );
}
