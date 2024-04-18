import { SetStateAction, useEffect, useState } from 'react';
import SearchForm from '../../components/searchForm/searchForm';
import MovieList from '../../components/movieList/movieList';
import MovieDetail from '../../components/movieDetail/movieDetail';
import PagesNavigation from '../../components/pagesNavigation/pagesNavigation';
import { getMovies } from '../../data/getMovies';

export default function HomePage() {
  const [search, setSearch] = useState('');
  const [allMoviesData, setAllMoviesData] = useState(Object);
  const [movies, setMovies] = useState([]);
  const [selectedMovie, setSelectedMovie] = useState(Number);
  const [pageNumber, setPageNumber] = useState(1);

  useEffect(() => {
    getMovies(search, setMovies, pageNumber, setAllMoviesData);
  }, [search, pageNumber]);

  const handleSearch = (e: { target: { value: SetStateAction<string> } }) => {
    setSearch(e.target.value);
  };

  const handleSelectedMovie = (movieId: number) => {
    setSelectedMovie(movieId);
  };

  const movieListElement = document.getElementById('movies_view');

  const handlePageNumberPlus = () => {
    setPageNumber((prevNumber) => prevNumber + 1);
    movieListElement?.scrollIntoView({ behavior: 'smooth' });
  };

  const handlePageNumberMinus = () => {
    setPageNumber((prevNumber) => prevNumber - 1);
    movieListElement?.scrollIntoView({ behavior: 'smooth' });
  };

  const totalPagesNumber = allMoviesData.total_pages;

  console.log('movies', movies);
  console.log('selectedMovie', selectedMovie);
  console.log('pageNumber', pageNumber);
  console.log('totalPagesNumber', totalPagesNumber);

  return (
    <main>
      {selectedMovie === 0 && (
        <>
          <h1>Find your movie</h1>
          <SearchForm searchValue={search} searchFunction={handleSearch} />
          <MovieList
            movieValues={movies}
            handleSelectedMovie={handleSelectedMovie}
          />
          {search.length > 0 && (
            <PagesNavigation
              pageNumber={pageNumber}
              totalPagesNumber={totalPagesNumber}
              increasePage={handlePageNumberPlus}
              decreasePage={handlePageNumberMinus}
            />
          )}
        </>
      )}
      {selectedMovie > 0 && (
        <MovieDetail
          selectedMovieId={selectedMovie}
          setSelectedMovie={setSelectedMovie}
        />
      )}
    </main>
  );
}
