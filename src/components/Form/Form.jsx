import React, { useState, useEffect } from "react";
import styled from "styled-components";
import axios from "axios";

const API_KEY = "03b8572954325680265531140190fd2a";
const API_URL = "https://api.themoviedb.org/3/search/movie";

const StyledFormLabel = styled.label`
  color: black;
  display: flex;
  align-items: center;
  justify-content: start;
  font-size: calc(10px + 2vmin);
  margin: calc(10px + 2vmin);
`;

const Input = styled.input`
  width: 60%;
  height: calc(10px + 2vmin);
  &:not(:focus) {
    box-shadow: 0 0 2px 2px #ff6a00;
  }
`;

const MovieList = styled.ul`
  list-style: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
`;

const MovieItem = styled.li`
  margin: 20px;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 25%;
  font-size: 2vmin;
`;

const Poster = styled.img`
  height: 30vmin;
`;

export const Form = () => {
  const [query, setQuery] = useState("");
  const [page, setPage] = useState(1);
  const [movies, setMovies] = useState([]);
  const [hasMore, setHasMore] = useState(false);

  useEffect(() => {
    if (query.length > 0) {
      const fetchMovies = async () => {
        try {
          const response = await axios.get(API_URL, {
            params: {
              api_key: API_KEY,
              query: query,
            },
          });
          console.log(response.data.total_pages);
          setMovies(response.data.results);
        } catch (error) {
          console.error("Error fetching data:", error);
        }
      };

      fetchMovies();
    } else {
      setMovies([]);
    }
  }, [query]);

  const handleInputChange = (e) => {
    setQuery(e.target.value);
  };
  console.log(movies);

  return (
    <>
      <form onSubmit={(e) => e.preventDefault()}>
        <StyledFormLabel htmlFor="movie-input">
          What would you like to find?
        </StyledFormLabel>
        <Input
          id="movie-input"
          type="text"
          placeholder="Type your search here"
          value={query}
          onChange={handleInputChange}
        />
      </form>
      <MovieList>
        {movies.map((movie) => (
          <MovieItem key={movie.id}>
            <Poster
              src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
            />
            <h2>{movie.title}</h2>
            <p>{movie.overview}</p>
          </MovieItem>
        ))}
      </MovieList>
    </>
  );
};
