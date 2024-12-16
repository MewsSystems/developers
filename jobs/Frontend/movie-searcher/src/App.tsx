import './App.css';
import { MovieList } from './components/MovieList';
import { getMovies } from './services/movieService';

function App() {
  const movies = getMovies();
  return (
    <>
      <div className='flex flex-col items-center'>
        <header>
          <form>
            <input className='min-w-80' type='text' placeholder='Avengers, Batman, Superman, etc.' />
          </form>
        </header>
        <div>
          <MovieList movies={movies} />
        </div>
      </div>
    </>
  );
}

export default App;
