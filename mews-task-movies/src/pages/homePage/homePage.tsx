import { SetStateAction, useEffect, useState } from 'react';
import MovieCard from '../../components/movieCard/movieCard';

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
      <h1>Movies</h1>
      <form>
        <label htmlFor="movie">Search for the movie: </label>
        <input type="text" id="movie" value={search} onChange={handleSearch} />
      </form>
      <div>
        {movies.map((movie: any) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </div>
    </main>
  );
}
