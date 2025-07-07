import { Link } from "react-router-dom";
import { useMovieContext } from "../../contexts/MovieContext";
// import "./MovieList.css"; // Assuming you have a CSS file for styling
import { Pagination } from "../Pagination/Pagination";
import { MovieListContainer, MovieCard } from "./MovieListStyles";

export const MovieList = (searchQuery: { searchQuery: string }) => {
  const { movies, currentPage, setCurrentPage, itemsPerPage, totalPages } =
    useMovieContext();
  let filteredMovies = movies;

  if (!movies.length) {
    return <p>No movies found.</p>;
  }

  // Filter movies based on search query
  if (searchQuery.searchQuery && searchQuery.searchQuery.trim() !== "") {
    filteredMovies = movies.filter((movie) =>
      movie.title.toLowerCase().includes(searchQuery.searchQuery.toLowerCase())
    );
    if (!filteredMovies.length) {
      return <p>No movies found for "{searchQuery.searchQuery}".</p>;
    }
  }

  const startIndex = (currentPage - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  filteredMovies.slice(startIndex, endIndex);

  return (
    <>
      <MovieListContainer>
        {filteredMovies.map((movie) => (
          <Link to={`/movie-details/${movie.id}`} key={movie.id}>
            <MovieCard>
              <h2>
                {movie.title.length > 16
                  ? movie.title.slice(0, 16) + "..."
                  : movie.title}
              </h2>
              {movie.poster_path !== null ? (
                <img
                  src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                  alt={movie.title}
                />
              ) : (
                <img
                  src="https://image.tmdb.org/t/p/w500/gEk9KNMeCe0h6sL3ISldvy7vPvR.jpg"
                  alt="No Image Available"
                />
              )}
            </MovieCard>
          </Link>
        ))}
      </MovieListContainer>
      <Pagination
        currentPage={currentPage}
        onPageChange={setCurrentPage}
        totalPages={totalPages}
      />
    </>
  );
};
