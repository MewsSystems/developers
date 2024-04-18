import { SetStateAction, useEffect, useState } from 'react';
import SearchForm from '../../components/searchForm/searchForm';
import MovieList from '../../components/movieList/movieList';
import MovieDetail from '../../components/movieDetail/movieDetail';
import { getMovies } from '../../data/getMovies';

export default function HomePage() {
  const [search, setSearch] = useState('');
  const [movies, setMovies] = useState([]);
  const [selectedMovie, setSelectedMovie] = useState(Number);

  useEffect(() => {
    getMovies(search, setMovies);
  }, [search]);

  const handleSearch = (e: { target: { value: SetStateAction<string> } }) => {
    setSearch(e.target.value);
  };

  const handleSelectedMovie = (movieId: number) => {
    setSelectedMovie(movieId);
  };

  console.log('movies', movies);

  console.log('selectedMovie', selectedMovie);

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
