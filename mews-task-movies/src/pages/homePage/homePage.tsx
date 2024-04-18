import { SetStateAction, useEffect, useState } from 'react';
import SearchForm from '../../components/searchForm/searchForm';
import MovieList from '../../components/movieList/movieList';
import { getMovies } from '../../data/getMovies';

export default function HomePage() {
  const [movies, setMovies] = useState([]);
  const [search, setSearch] = useState('');

  useEffect(() => {
    getMovies(search, setMovies);
  }, [search]);

  const handleSearch = (e: { target: { value: SetStateAction<string> } }) => {
    setSearch(e.target.value);
  };

  console.log('movies', movies);

  return (
    <main>
      <h1>Find your movie</h1>
      <SearchForm searchValue={search} searchFunction={handleSearch} />
      <MovieList movieValues={movies} />
    </main>
  );
}
