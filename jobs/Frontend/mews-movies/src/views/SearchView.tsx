import React, { useState } from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { searchMovies, getMovieImageUrl } from "../api/tmdb";
import { Movie } from "../types/Movie";

const SearchContainer = styled.div`
  padding: 2rem;
`;

const SearchInput = styled.input`
  padding: 0.5rem;
  font-size: 1rem;
`;

const MovieList = styled.div`
  margin-top: 2rem;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 1rem;
`;

const MovieCard = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const MovieImage = styled.img`
  width: 100%;
  height: auto;
`;

const MovieTitle = styled.h3`
  font-size: 1.2rem;
  margin: 0.5rem 0;
`;

const MovieReleaseDate = styled.p`
  font-size: 0.9rem;
  color: #555;
`;

const MovieRating = styled.p`
  font-size: 1rem;
  color: #000;
`;

const SearchView: React.FC = () => {
  const [query, setQuery] = useState("");
  const [movies, setMovies] = useState<Movie[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const handleSearch = async () => {
    setError(null);
    try {
      const data = await searchMovies(query, page);
      setMovies(data.results);
      setTotalPages(data.total_pages);
    } catch (err) {
      setError("Error fetching movies");
    }
  };

  const handleNextPage = () => {
    if (page < totalPages) {
      setPage((prevPage) => prevPage + 1);
      handleSearch();
    }
  };

  const handlePrevPage = () => {
    if (page > 1) {
      setPage((prevPage) => prevPage - 1);
      handleSearch();
    }
  };

  return (
    <SearchContainer>
      <SearchInput
        type="text"
        placeholder="Search for a movie..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />
      <button onClick={handleSearch}>Search</button>
      {error && <p>{error}</p>}
      <MovieList>
        {movies.length > 0 ? (
          movies.map((movie) => (
            <MovieCard key={movie.id}>
              <Link to={`/movie/${movie.id}`}>
                <MovieImage
                  src={getMovieImageUrl(movie.poster_path)}
                  alt={movie.title}
                />
                <MovieTitle>{movie.title}</MovieTitle>
                <MovieReleaseDate>{movie.release_date}</MovieReleaseDate>
                <MovieRating>{movie.vote_average}</MovieRating>
              </Link>
            </MovieCard>
          ))
        ) : (
          <p>No movies found</p>
        )}
      </MovieList>
      <div>
        {page > 1 && <button onClick={handlePrevPage}>Previous</button>}
        {page < totalPages && <button onClick={handleNextPage}>Next</button>}
      </div>
    </SearchContainer>
  );
};

export default SearchView;
