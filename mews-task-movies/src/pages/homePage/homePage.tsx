import { SetStateAction, useEffect, useState } from 'react';
import SearchForm from '../../components/searchForm/searchForm';
import MovieList from '../../components/movieList/movieList';

export default function HomePage() {
  const [movies, setMovies] = useState([]);
  const [search, setSearch] = useState('');

  useEffect(() => {
    const getMovies = async () => {
      const options = {
        method: 'GET',
        headers: {
          accept: 'application/json',
        },
      };
      const response = await fetch(
        `https://api.themoviedb.org/3/search/movie?query=${search}&api_key=03b8572954325680265531140190fd2a`,
        options,
      );
      const moviesData = await response.json();

      console.log('moviesData', moviesData);
      setMovies(moviesData.results);
      console.log('moviesData.result', moviesData.results);
    };
    getMovies();
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
